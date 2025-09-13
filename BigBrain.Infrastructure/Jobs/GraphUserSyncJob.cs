using BigBrain.Core.Interfaces;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace BigBrain.Infrastructure.Jobs
{
    public class GraphUserSyncJob
    {
        private readonly IUserSyncService _syncService;
        private readonly ILogger<GraphUserSyncJob> _logger;

        public GraphUserSyncJob(IUserSyncService syncService, ILogger<GraphUserSyncJob> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        public async Task ExecuteAsync(PerformContext context)
        {
            _logger.LogInformation("Initiating Microsoft Graph user synchronization...");
            await _syncService.ExecuteAsync(context);
            _logger.LogInformation("Microsoft Graph user synchronization finished successfully.");
        }
    }
}
