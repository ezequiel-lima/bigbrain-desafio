using BigBrain.Core.Interfaces;
using BigBrain.Infrastructure.Graph.Auth;
using BigBrain.Infrastructure.Users;

namespace BigBrain.Web.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static WebApplicationBuilder ResolveDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IGraphAuthProvider, GraphAuthProvider>();
            builder.Services.AddScoped<IGraphUserService, GraphUserService>();

            return builder;
        }
    }
}
