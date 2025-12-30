namespace SmartERP.Application.ProductCategories.Dtos;

public class CreateProductCategoryRequest
{
    public string CategoryName { get; set; } = default!;
    public string? Description { get; set; }
}