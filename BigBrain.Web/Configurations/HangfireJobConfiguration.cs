using BigBrain.Infrastructure.Jobs;
using Hangfire;

namespace BigBrain.Web.Configurations
{
    public static class HangfireJobConfiguration
    {
        public static void Register()
        {
            RecurringJob.AddOrUpdate(
                "graph-chained-sync",
                () => HangfireJobConfiguration.RunChainedSync(),
                "0 2 * * *"
            );
        }

        public static void RunChainedSync()
        {
            var GraphUserSyncJobId = BackgroundJob.Enqueue<GraphUserSyncJob>(
                job => job.ExecuteAsync(null)
            );

            BackgroundJob.ContinueJobWith<GraphUserCalendarSyncJob>(
                GraphUserSyncJobId,
                job => job.ExecuteAsync(null)
            );
        }
    }
}
