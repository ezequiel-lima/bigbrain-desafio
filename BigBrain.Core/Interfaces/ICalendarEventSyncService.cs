using Hangfire.Server;

namespace BigBrain.Core.Interfaces
{
    public interface ICalendarEventSyncService
    {
        Task ExecuteAsync(PerformContext? context);
    }
}
