using SmartERP.Domain.Aggregates.ProductCategoryAggregate;

namespace SmartERP.Application.ProductCategories.Repositories;

public interface IProductCategoryRepository
{
    Task InsertAsync(ProductCategory category, CancellationToken ct);
    Task<ProductCategory?> GetByUidAsync(Guid uid, CancellationToken ct);
    Task<List<ProductCategory>> GetListAsync(CancellationToken ct);
    Task<bool> UpdateAsync(ProductCategory category, CancellationToken ct);
    Task<bool> DeleteAsync(Guid uid, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeUid, CancellationToken ct);
}