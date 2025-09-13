using BigBrain.Infrastructure.Persistence.Context;
using BigBrain.Web.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BigBrainDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.ResolveDependencies();

builder.AddHangfireConfiguration();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseHangfireBoard();

HangfireJobConfiguration.Register();

app.UseAuthorization();

app.MapControllers();

app.Run();