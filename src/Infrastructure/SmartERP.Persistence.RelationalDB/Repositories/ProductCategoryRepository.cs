using Microsoft.EntityFrameworkCore;
using SmartERP.Application.ProductCategories.Interfaces;
using SmartERP.Domain.Aggregates.ProductCategoryAggregate;
using SmartERP.Persistence.RelationalDB.Common.Interfaces;

namespace SmartERP.Persistence.RelationalDB.Repositories;

public class ProductCategoryRepository(SmartERPDbContext db, ISequenceService sequenceService)
    : IProductCategoryRepository
{
    public async Task InsertAsync(ProductCategory category, CancellationToken ct)
    {
        var businessId = await sequenceService.NextAsync(
            "PC",
            "ProductCategorySeq",
            ct);
        category.SetProductCategoryId(businessId);
        db.ProductCategories.Add(category);
        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
    {
        return await db.ProductCategories
            .AnyAsync(x => x.CategoryName == name, ct);
    }
}