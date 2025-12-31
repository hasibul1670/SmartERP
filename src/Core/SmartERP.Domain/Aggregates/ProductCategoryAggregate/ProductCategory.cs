using SmartERP.Domain.Common;

namespace SmartERP.Domain.Aggregates.ProductCategoryAggregate;

public class ProductCategory
{
    public int Id { get; private set; } // keep identity
    public Guid Uid { get; private set; }
    public string ProductCategoryId { get; private set; } = null!;
    public string? CategoryName { get; set; }
    public string? Description { get; set; }

    public static Result<ProductCategory> Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<ProductCategory>.Failure("Category name is required.");

        return Result<ProductCategory>.Success(new ProductCategory
        {
            Uid = Guid.NewGuid(),
            CategoryName = name.Trim(),
            Description = description?.Trim()
        });
    }

    public void SetProductCategoryId(string productCategoryId)
    {
        ProductCategoryId = productCategoryId;
    }
}