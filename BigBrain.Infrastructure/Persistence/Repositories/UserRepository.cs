using BigBrain.Core.Interfaces;
using BigBrain.Core.Models;
using BigBrain.Infrastructure.Persistence.Context;
using BigBrain.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BigBrain.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BigBrainDbContext _context;

        public UserRepository(BigBrainDbContext context)
        {
            _context = context;
        }

        public async Task DeleteUsersAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Users]");
        }

        public async Task InsertUsersAsync(IList<UserModel> users)
        {
            if (!users.Any()) return;

            var entities = users.Select(user => (UserEntity)user);

            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<List<(Guid Id, string UserPrincipalName)>> GetUserBatchAsync(int skip, int take)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => !string.IsNullOrEmpty(u.UserPrincipalName))
                .OrderBy(u => u.Id)
                .Skip(skip)
                .Take(take)
                .Select(u => new ValueTuple<Guid, string>(u.Id, u.UserPrincipalName!))
                .ToListAsync();
        }
    }
}
