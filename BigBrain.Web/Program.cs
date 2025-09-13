using BigBrain.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.ResolveDependencies();

builder.AddHangfireConfiguration();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseHangfireBoard();

app.UseAuthorization();

app.MapControllers();

app.Run();
