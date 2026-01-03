namespace SmartERP.Application.ProductCategories.Dtos;

public class UpdateProductCategoryRequest
{
    public string CategoryName { get; set; } = default!;
    public string? Description { get; set; }
}
