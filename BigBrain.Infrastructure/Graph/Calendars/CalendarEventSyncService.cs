using BigBrain.Core.Configurations;
using BigBrain.Core.Interfaces;
using BigBrain.Core.Models;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace BigBrain.Infrastructure.Graph.Calendars
{
    public class CalendarEventSyncService : ICalendarEventSyncService
    {
        private readonly IGraphCalendarEventService _graphCalendarService;
        private readonly ICalendarEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CalendarEventSyncService> _logger;
        private readonly CalendarEventSyncSettings _settings;

        public CalendarEventSyncService(
            IGraphCalendarEventService graphCalendarService,
            ICalendarEventRepository eventRepository,
            IUserRepository userRepository,
            ILogger<CalendarEventSyncService> logger,
            IOptions<CalendarEventSyncSettings> settings)
        {
            _graphCalendarService = graphCalendarService;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task ExecuteAsync(PerformContext? context)
        {
            var stopwatch = Stopwatch.StartNew();
            context?.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting parallel calendar event synchronization...");

            await _eventRepository.DeleteCalendarEventsAsync();
            context?.WriteLine($"[{DateTime.Now:HH:mm:ss}] Successfully deleted existing calendar event.");

            var startDate = _settings.StartDate;
            var endDate = _settings.EndDate;
            var batchSize = _settings.BatchSize;
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = _settings.MaxDegreeOfParallelism };

            int skip = 0;
            long totalCalendarEventsSaved = 0;
            int batchNumber = 1;

            while (true)
            {
                var userBatch = await _userRepository.GetUserBatchAsync(skip, batchSize);

                if (userBatch.Count == 0)
                    break;

                var allEvents = new ConcurrentBag<CalendarEventModel>();

                await Parallel.ForEachAsync(userBatch, parallelOptions, async (user, cancellationToken) =>
                {
                    try
                    {
                        var calendarEvents = await _graphCalendarService
                            .GetUserCalendarEventsAsync(user.UserPrincipalName, startDate, endDate);

                        if (calendarEvents.Count > 0)
                        {
                            foreach (var graphCalendarEventDto in calendarEvents)
                            {
                                graphCalendarEventDto.UserId = user.Id;
                                totalCalendarEventsSaved++;                                
                                allEvents.Add((CalendarEventModel)graphCalendarEventDto);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"[{DateTime.Now:HH:mm:ss}] Error syncing calendar events for user {user.UserPrincipalName}. " +
                            $"Skipping user");
                    }
                });

                context?.WriteLine($"[{DateTime.Now:HH:mm:ss}] Batch {batchNumber++}: calendar events saved. " +
                    $"Total: {totalCalendarEventsSaved}.");
                await _eventRepository.CalendarEventsInsertAsync(allEvents.ToList());
                skip += batchSize;
            }
        }
    }
}
