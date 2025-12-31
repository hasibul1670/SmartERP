using SmartERP.Domain.Common;

namespace SmartERP.Domain.Aggregates.ProductCategoryAggregate;

public class ProductCategory
{
    private ProductCategory()
    {
    }

    public int Id { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }

    public static Result<ProductCategory> Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<ProductCategory>.Failure("Category name is required.");

        return Result<ProductCategory>.Success(new ProductCategory
        {
            CategoryName = name.Trim(),
            Description = description?.Trim()
        });
    }
}