using SmartERP.Domain.Aggregates.ProductCategoryAggregate;

namespace SmartERP.Persistence.RelationalDB.Repositories;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly SmartERPDbContext _db;

    public ProductCategoryRepository(SmartERPDbContext db)
    {
        _db = db;
    }

    public async Task InsertAsync(ProductCategory category, CancellationToken ct)
    {
        _db.ProductCategories.Add(category);
        await _db.SaveChangesAsync(ct);
    }
}