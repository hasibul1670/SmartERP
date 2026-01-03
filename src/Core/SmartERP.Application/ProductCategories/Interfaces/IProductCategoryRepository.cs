using SmartERP.Domain.Aggregates.ProductCategoryAggregate;

namespace SmartERP.Application.ProductCategories.Interfaces;

public interface IProductCategoryRepository
{
    Task InsertAsync(ProductCategory category, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
}