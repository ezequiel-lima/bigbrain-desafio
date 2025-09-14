using BigBrain.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
namespace BigBrain.Infrastructure.Persistence.Context
{
    [ExcludeFromCodeCoverage]
    public class BigBrainDbContext : DbContext
    {
        public BigBrainDbContext(DbContextOptions<BigBrainDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CalendarEventEntity> CalendarEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BigBrainDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
