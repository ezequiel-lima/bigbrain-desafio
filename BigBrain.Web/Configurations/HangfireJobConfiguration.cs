using BigBrain.Infrastructure.Jobs;
using Hangfire;

namespace BigBrain.Web.Configurations
{
    public static class HangfireJobConfiguration
    {
        public static void Register()
        {
            RecurringJob.AddOrUpdate<GraphUserSyncJob>(
                "sync-graph-users",
                job => job.ExecuteAsync(null),
                Cron.Hourly);
        }
    }
}
