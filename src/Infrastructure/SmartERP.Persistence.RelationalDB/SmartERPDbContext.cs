using Microsoft.EntityFrameworkCore;
using SmartERP.Domain.Aggregates.ProductCategoryAggregate;

namespace SmartERP.Persistence.RelationalDB;

public class SmartERPDbContext(DbContextOptions<SmartERPDbContext> options) : DbContext(options)
{
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartERPDbContext).Assembly);
    }
}