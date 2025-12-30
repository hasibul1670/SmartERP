using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using SmartERP.Persistence.RelationalDB;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6001");
builder.Services.AddControllers();
builder.Services.AddDbContext<SmartERPDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SmartERP")));
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// ✅ Serilog
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) 
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] {RequestMethod} {RequestPath} → {StatusCode} ({Elapsed:0.0000} ms){NewLine}");
});

var app = builder.Build();
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diag, http) =>
    {
        diag.Set("RequestHost", http.Request.Host.Value);
        diag.Set("RequestScheme", http.Request.Scheme);
    };
});
app.MapControllers();
app.MapControllerRoute(
    "api",
    "api/v1/{controller}/{action?}/{id?}");

//Test API
app.MapGet("/api/v1/status", () => Results.Ok("Server is ON!"));
app.MapGet("/api/v1/db-check", async (SmartERPDbContext db) =>
{
    var ok = await db.Database.CanConnectAsync();
    return Results.Ok(new { ok });
});

app.Run();