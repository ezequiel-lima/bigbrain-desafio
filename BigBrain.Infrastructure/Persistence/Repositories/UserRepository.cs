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

        public async Task TruncateUsersAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Users");
        }

        public async Task InsertUsersAsync(IList<UserModel> users)
        {
            var entities = users.Select(user => (UserEntity)user);

            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
    }
}
