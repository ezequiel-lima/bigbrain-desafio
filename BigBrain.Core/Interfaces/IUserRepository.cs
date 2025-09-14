using BigBrain.Core.Models;

namespace BigBrain.Core.Interfaces
{
    public interface IUserRepository
    {
        Task DeleteUsersAsync();
        Task InsertUsersAsync(IList<UserModel> users);
        Task<List<(Guid Id, string UserPrincipalName)>> GetUserBatchAsync(int skip, int take);
    }
}
