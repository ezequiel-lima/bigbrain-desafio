using BigBrain.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BigBrain.Web.Configurations
{
    public static class PersistenceConfiguration
    {
        public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<BigBrainDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            return builder;
        }
    }
}
