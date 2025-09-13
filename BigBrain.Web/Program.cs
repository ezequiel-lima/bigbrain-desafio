using BigBrain.Core.Interfaces;
using BigBrain.Infrastructure.Graph.Auth;
using BigBrain.Infrastructure.Users;
using BigBrain.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IGraphAuthProvider, GraphAuthProvider>();
builder.Services.AddScoped<IGraphUserService, GraphUserService>();

builder.AddHangfireConfiguration();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseHangfireBoard();

app.UseAuthorization();

app.MapControllers();

app.Run();
