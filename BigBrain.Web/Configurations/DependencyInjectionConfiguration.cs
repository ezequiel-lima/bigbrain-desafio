using BigBrain.Core.Interfaces;
using BigBrain.Infrastructure.Graph.Auth;
using BigBrain.Infrastructure.Graph.Calendars;
using BigBrain.Infrastructure.Graph.Users;
using BigBrain.Infrastructure.Persistence.Repositories;

namespace BigBrain.Web.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static WebApplicationBuilder ResolveDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IGraphAuthProvider, GraphAuthProvider>();

            builder.Services.AddScoped<IGraphUserService, GraphUserService>();
            builder.Services.AddScoped<IUserSyncService, UserSyncService>();
            builder.Services.AddScoped<IGraphCalendarEventService, GraphCalendarEventService>();
            builder.Services.AddScoped<ICalendarEventSyncService, CalendarEventSyncService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();

            return builder;
        }
    }
}
