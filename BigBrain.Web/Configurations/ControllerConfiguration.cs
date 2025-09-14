using BigBrain.Core.Configurations;

namespace BigBrain.Web.Configurations
{
    public static class ControllerConfiguration
    {
        public static WebApplicationBuilder AddControllerSetup(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.Configure<CalendarEventSyncSettings>(
                builder.Configuration.GetSection("CalendarEventSync"));

            return builder;
        }
    }
}
