using SmartERP.Application.Interfaces;
using SmartERP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using SmartERP.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SmartERP")));

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapGet("/db-check", async (AppDbContext db) =>
{
    var ok = await db.Database.CanConnectAsync();
    return Results.Ok(new { ok });
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
