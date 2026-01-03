using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartERP.Api.Extensions;
using SmartERP.Application.ProductCategories.Commands.UpdateProductCategory;
using SmartERP.Application.ProductCategories.Dtos;

namespace SmartERP.Api.Features.ProductCategories.Endpoints;

public sealed class UpdateProductCategoryEndpoint(IMediator mediator) : ProductCategoriesEndpointBase
{
    [HttpPut("{uid:guid}")]
    public async Task<ActionResult> Put(Guid uid, [FromBody] UpdateProductCategoryRequest model, CancellationToken ct)
    {
        var command = new UpdateProductCategoryCommand(uid, model.CategoryName, model.Description);
        var result = await mediator.Send(command, ct);
        if (!result.IsSuccess)
        {
            if (result.Errors.TryGetValue("not_found", out var message))
                return this.ApiNotFound(message);

            return this.ApiBadRequest("Validation failed.", result.Errors);
        }

        return NoContent();
    }
}