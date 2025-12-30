using Microsoft.AspNetCore.Mvc;
using SmartERP.Application.ProductCategories.Dtos;

namespace SmartERP.Api.Controllers;

[ApiController]
[Route("api/v1/product-categories")]
public class ProductCategoriesController : ControllerBase
{
    // private readonly IProductCategoryService _service;
    //
    // public ProductCategoriesController(IProductCategoryService service)
    // {
    //     _service = service;
    // }

    // POST: /api/v1/product-categories
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductCategoryRequest request,
        CancellationToken ct)
    {
        // DEMO ONLY (service commented)
        // var created = await _service.CreateAsync(request, ct);

        return Ok(new
        {
            message = "Product category created successfully (demo)",
            data = new
            {
                id = 1,
                categoryName = request.CategoryName,
                description = request.Description
            }
        });
    }

    // GET: /api/v1/product-categories/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        [FromRoute] int id,
        CancellationToken ct)
    {
        // DEMO ONLY
        // var item = await _service.GetByIdAsync(id, ct);

        return Ok(new
        {
            message = "Single product category fetched successfully (demo)",
            data = new
            {
                id,
                categoryName = "Electronics",
                description = "Demo category"
            }
        });
    }

    // GET: /api/v1/product-categories
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        // DEMO ONLY
        // var list = await _service.GetAllAsync(ct);

        return Ok(new
        {
            message = "All product categories fetched successfully (demo)",
            data = new[]
            {
                new { id = 1, categoryName = "Electronics", description = "Demo data" },
                new { id = 2, categoryName = "Groceries", description = "Demo data" }
            }
        });
    }

    // PUT: /api/v1/product-categories/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateProductCategoryRequest request,
        CancellationToken ct)
    {
        // DEMO ONLY
        // var updated = await _service.UpdateAsync(id, request, ct);

        return Ok(new
        {
            message = "Product category updated successfully (demo)",
            data = new
            {
                id,
                categoryName = request.CategoryName,
                description = request.Description
            }
        });
    }

    // DELETE: /api/v1/product-categories/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        [FromRoute] int id,
        CancellationToken ct)
    {
        // DEMO ONLY
        // var ok = await _service.DeleteAsync(id, ct);

        return Ok(new
        {
            message = $"Product category with id {id} deleted successfully (demo)"
        });
    }
}