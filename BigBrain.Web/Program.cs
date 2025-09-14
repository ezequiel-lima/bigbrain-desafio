using BigBrain.Core.Configurations;
using BigBrain.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddControllerSetup();

builder.AddPersistence();

builder.ResolveDependencies();

builder.AddHangfireConfiguration();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseHangfireBoard();

HangfireJobConfiguration.Register();

app.UseAuthorization();

app.MapControllers();

app.Run();
