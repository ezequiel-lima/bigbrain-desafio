using BigBrain.Core.Interfaces;
using BigBrain.Core.Models;
using Hangfire.Console;
using Hangfire.Server;
using System.Diagnostics;

namespace BigBrain.Infrastructure.Graph.Users
{
    public class UserSyncService : IUserSyncService
    {
        private readonly IGraphUserService _graphUserService;
        private readonly IUserRepository _userRepository;

        public UserSyncService(IGraphUserService graphUserService, IUserRepository userRepository)
        {
            _graphUserService = graphUserService;
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(PerformContext? context = null)
        {
            var stopwatch = Stopwatch.StartNew();
            context?.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting full user synchronization...");

            await _userRepository.DeleteUsersAsync();
            context?.WriteLine($"[{DateTime.Now:HH:mm:ss}] User table successfully truncated.");

            long totalUsersSaved = 0;
            int batchNumber = 1;
            string? nextLink = null;

            do
            {
                var batchDto = await _graphUserService.GetUsersAsync(nextLink);
                if (batchDto?.Users == null || !batchDto.Users.Any())
                {
                    break;
                }

                var domainUsers = batchDto.Users.Select(dto => (UserModel)dto).ToList();

                await _userRepository.InsertUsersAsync(domainUsers);

                totalUsersSaved += domainUsers.Count;
                context?.WriteLine($"[{DateTime.Now:HH:mm:ss}] Batch {batchNumber++}: {domainUsers.Count} users saved. Total: {totalUsersSaved}.");

                nextLink = batchDto.NextLink;

            } while (!string.IsNullOrEmpty(nextLink));

            stopwatch.Stop();
            context?.WriteLine($"[{DateTime.Now:HH:mm:ss}] Synchronization finished in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
            context?.WriteLine($"Total of {totalUsersSaved} users saved to the database.");
        }
    }
}
