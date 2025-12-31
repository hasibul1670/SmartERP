namespace SmartERP.Domain.Aggregates.ProductCategoryAggregate;

public interface IProductCategoryRepository
{
    Task InsertAsync(ProductCategory category, CancellationToken ct);
}