using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartERP.Api.Extensions;
using SmartERP.Application.ProductCategories.Commands.CreateProductCategory;
using SmartERP.Application.ProductCategories.Dtos;

namespace SmartERP.Api.Features.ProductCategories.Endpoints;

public sealed class CreateProductCategoryEndpoint(IMediator mediator) : ProductCategoriesEndpointBase
{
    // private readonly IMediator _mediator;

    // public CreateProductCategoryEndpoint(IMediator mediator)=> _mediator = mediator;
    [HttpPost]
    public async Task<ActionResult> Post(CreateProductCategoryRequest model, CancellationToken ct)
    {
        var command = new CreateProductCategoryCommand(model.CategoryName, model.Description);
        var result = await mediator.Send(command, ct);
        if (!result.IsSuccess)
            return this.ApiBadRequest("Validation failed.", result.Errors);
        var location = $"/api/v1/product-categories/{result.Value}";
        var responseBody = new
        {
            id = result.Value,
            categoryName = model.CategoryName,
            description = model.Description
        };
        return this.ApiCreated(location, responseBody, "Product category created");
    }
}