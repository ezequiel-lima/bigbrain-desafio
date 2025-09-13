using Hangfire.Server;

namespace BigBrain.Core.Interfaces
{
    public interface IUserSyncService
    {
        Task ExecuteAsync(PerformContext? context = null);
    }
}
