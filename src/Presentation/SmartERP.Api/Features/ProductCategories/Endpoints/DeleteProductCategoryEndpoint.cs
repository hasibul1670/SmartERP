using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartERP.Api.Extensions;
using SmartERP.Application.ProductCategories.Commands.DeleteProductCategory;

namespace SmartERP.Api.Features.ProductCategories.Endpoints;

public sealed class DeleteProductCategoryEndpoint(IMediator mediator) : ProductCategoriesEndpointBase
{
    [HttpDelete("{uid:guid}")]
    public async Task<ActionResult> Delete(Guid uid, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteProductCategoryCommand(uid), ct);
        if (!result.IsSuccess)
            return this.ApiNotFound("Product category not found.");

        return NoContent();
    }
}