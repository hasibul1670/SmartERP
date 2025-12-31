using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartERP.Application.ProductCategories.Commands.CreateProductCategory;
using SmartERP.Application.ProductCategories.Dtos;

namespace SmartERP.Api.Features.ProductCategories.Endpoints;

public sealed class CreateProductCategoryEndpoint : ProductCategoriesEndpointBase
{
    private readonly IMediator _mediator;

    public CreateProductCategoryEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    // 🔑 THIS METHOD CREATES THE ROUTE
    [HttpPost]
    public async Task<ActionResult> Post(CreateProductCategoryRequest model, CancellationToken ct)
    {
        var command = new CreateProductCategoryCommand(model.CategoryName, model.Description);
        var result = await _mediator.Send(command, ct);

        if (!result.IsSuccess)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Key, error.Value);

            return ValidationProblem(ModelState);
        }

        return Created($"/api/v1/product-categories/{result.Value}", model);
    }
}