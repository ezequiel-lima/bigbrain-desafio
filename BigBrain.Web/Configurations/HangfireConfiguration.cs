using Hangfire;
using Hangfire.Console;

namespace BigBrain.Web.Configurations
{
    public static class HangfireConfiguration
    {
        public static WebApplicationBuilder AddHangfireConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"))
                .UseConsole());

            builder.Services.AddHangfireServer();

            return builder;
        }

        public static WebApplication UseHangfireBoard(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseHangfireDashboard();
            }

            return app;
        }
    }
}
