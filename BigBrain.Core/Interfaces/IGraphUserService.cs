using BigBrain.Core.Dtos;

namespace BigBrain.Core.Interfaces
{
    public interface IGraphUserService
    {
        Task<GraphUserBatchDto> GetUsersAsync(string? nextLink = null);
    }
}
