using Microsoft.EntityFrameworkCore;
using SmartERP.Application.ProductCategories.Repositories;
using SmartERP.Domain.Aggregates.ProductCategoryAggregate;
using SmartERP.Persistence.RelationalDB.Common.Interfaces;

namespace SmartERP.Persistence.RelationalDB.ProductCategories.Repositories;

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

    public async Task<ProductCategory?> GetByUidAsync(Guid uid, CancellationToken ct)
    {
        return await db.ProductCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Uid == uid, ct);
    }

    public async Task<List<ProductCategory>> GetListAsync(CancellationToken ct)
    {
        return await db.ProductCategories
            .AsNoTracking()
            .OrderBy(x => x.CategoryName)
            .ToListAsync(ct);
    }

    public async Task<bool> UpdateAsync(ProductCategory category, CancellationToken ct)
    {
        db.ProductCategories.Update(category);
        return await db.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(Guid uid, CancellationToken ct)
    {
        var existing = await db.ProductCategories.FirstOrDefaultAsync(x => x.Uid == uid, ct);
        if (existing is null) return false;

        db.ProductCategories.Remove(existing);
        return await db.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeUid, CancellationToken ct)
    {
        var query = db.ProductCategories.AsQueryable();
        if (excludeUid.HasValue)
            query = query.Where(x => x.Uid != excludeUid.Value);

        return await query.AnyAsync(x => x.CategoryName == name, ct);
    }
}