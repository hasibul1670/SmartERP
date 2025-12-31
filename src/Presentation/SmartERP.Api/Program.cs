using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using SmartERP.Api.Conventions;
using SmartERP.Application.ProductCategories.Commands.CreateProductCategory;
using SmartERP.Domain.Aggregates.ProductCategoryAggregate;
using SmartERP.Persistence.RelationalDB;
using SmartERP.Persistence.RelationalDB.Repositories;
// ✅ add these (adjust namespaces to yours)

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:6001");

// ✅ Keep only ONE AddControllers
builder.Services.AddControllers(options =>
{
    options.Conventions.Insert(0, new GlobalRoutePrefixConvention("api/v1"));
});

builder.Services.AddDbContext<SmartERPDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SmartERP")));

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// ✅ MediatR registration (this fixes IMediator error)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCategoryCommand).Assembly));

// ✅ Repository registration (next required dependency)
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

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

// Test API
app.MapGet("/api/v1/status", () => Results.Ok("Server is ON!"));
app.MapGet("/api/v1/db-check", async (SmartERPDbContext db) =>
{
    var ok = await db.Database.CanConnectAsync();
    return Results.Ok(new { ok });
});

app.Run();