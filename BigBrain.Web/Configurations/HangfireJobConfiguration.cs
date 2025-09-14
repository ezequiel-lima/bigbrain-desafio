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
                "0 2 * * *"
            );

            RecurringJob.AddOrUpdate<GraphUserCalendarSyncJob>(
                "sync-graph-users-calendars",
                job => job.ExecuteAsync(null),
                Cron.Never() 
            );
        }
    }
}
