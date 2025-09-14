using BigBrain.Core.Interfaces;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace BigBrain.Infrastructure.Jobs
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 10, 30, 60 }, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public class GraphUserSyncJob
    {
        private readonly IUserSyncService _syncService;
        private readonly ILogger<GraphUserSyncJob> _logger;

        public GraphUserSyncJob(IUserSyncService syncService, ILogger<GraphUserSyncJob> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        public async Task ExecuteAsync(PerformContext? context = null)
        {
            _logger.LogInformation("Initiating Microsoft Graph user synchronization...");
            await _syncService.ExecuteAsync(context);
            _logger.LogInformation("Microsoft Graph user synchronization finished successfully.");

            BackgroundJob.Enqueue<GraphUserCalendarSyncJob>(job => job.ExecuteAsync(null));
        }
    }
}
