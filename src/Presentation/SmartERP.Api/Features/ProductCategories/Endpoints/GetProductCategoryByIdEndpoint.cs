using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartERP.Api.Extensions;
using SmartERP.Application.ProductCategories.Queries.GetProductCategoryById;

namespace SmartERP.Api.Features.ProductCategories.Endpoints;

public sealed class GetProductCategoryByIdEndpoint(IMediator mediator) : ProductCategoriesEndpointBase
{
    [HttpGet("{uid:guid}")]
    public async Task<ActionResult> Get(Guid uid, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductCategoryByIdQuery(uid), ct);
        if (result is null)
            return this.ApiNotFound("Product category not found.");

        return this.ApiOk(result, "Product category fetched");
    }
}