using BigBrain.Core.Interfaces;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace BigBrain.Infrastructure.Jobs
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 10, 30, 60 }, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public class GraphUserCalendarSyncJob
    {
        private readonly ICalendarEventSyncService _syncService;
        private readonly ILogger<GraphUserCalendarSyncJob> _logger;

        public GraphUserCalendarSyncJob(ICalendarEventSyncService syncService, ILogger<GraphUserCalendarSyncJob> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        public async Task ExecuteAsync(PerformContext? context = null)
        {
            _logger.LogInformation("Initiating Microsoft Graph calendar events synchronization...");
            await _syncService.ExecuteAsync(context);
            _logger.LogInformation("Microsoft Graph calendar events synchronization finished successfully.");
        }
    }
}
