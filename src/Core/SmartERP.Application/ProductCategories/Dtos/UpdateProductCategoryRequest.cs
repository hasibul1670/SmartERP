namespace SmartERP.Application.ProductCategories.Dtos;

public class UpdateProductCategoryRequest
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = default!;
    public string? Description { get; set; }
}