using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartERP.Api.Extensions;
using SmartERP.Application.ProductCategories.Queries.GetProductCategoryList;

namespace SmartERP.Api.Features.ProductCategories.Endpoints;

public sealed class ProductCategoriesListEndpoint(IMediator mediator) : ProductCategoriesEndpointBase
{
    [HttpGet]
    public async Task<ActionResult> Get(CancellationToken ct)
    {
        var results = await mediator.Send(new GetProductCategoryListQuery(), ct);
        return this.ApiOk(results, "Product categories fetched");
    }
}
