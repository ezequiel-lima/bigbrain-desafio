using BigBrain.Core.Models;

namespace BigBrain.Core.Interfaces
{
    public interface IUserRepository
    {
        Task TruncateUsersAsync();
        Task InsertUsersAsync(IList<UserModel> users);
    }
}
