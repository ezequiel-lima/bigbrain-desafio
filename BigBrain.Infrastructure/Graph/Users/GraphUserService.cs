using BigBrain.Core.Dtos;
using BigBrain.Core.Interfaces;
using BigBrain.Infrastructure.Graph.Auth;
using Microsoft.Graph;

namespace BigBrain.Infrastructure.Graph.Users
{
    public class GraphUserService : IGraphUserService
    {
        private readonly GraphServiceClient _graphClient;

        public GraphUserService(IGraphAuthProvider authProvider)
        {
            _graphClient = authProvider.CreateClient();
        }

        public async Task<GraphUserBatchDto> GetUsersAsync(string? nextLink = null)
        {
            var response = string.IsNullOrEmpty(nextLink)
                ? await _graphClient.Users.GetAsync(config =>
                {
                    config.QueryParameters.Top = 999;
                })
                : await _graphClient.Users.WithUrl(nextLink).GetAsync();

            var users = response?.Value?
                .Select(user => (GraphUserDto)user)
                .ToList() ?? new();

            return new GraphUserBatchDto
            {
                Users = users,
                NextLink = response?.OdataNextLink
            };
        }
    }
}
