namespace BigBrain.Web.Configurations
{
    public static class ControllerConfiguration
    {
        public static WebApplicationBuilder AddControllerSetup(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            return builder;
        }
    }
}
