using BigBrain.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
namespace BigBrain.Infrastructure.Persistence.Context
{
    public class BigBrainDbContext : DbContext
    {
        public BigBrainDbContext(DbContextOptions<BigBrainDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

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
