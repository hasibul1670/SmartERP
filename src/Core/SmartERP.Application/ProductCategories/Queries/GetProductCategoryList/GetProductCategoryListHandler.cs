using MediatR;
using SmartERP.Application.ProductCategories.Dtos;
using SmartERP.Application.ProductCategories.Repositories;

namespace SmartERP.Application.ProductCategories.Queries.GetProductCategoryList;

public sealed class GetProductCategoryListHandler(IProductCategoryRepository repository)
    : IRequestHandler<GetProductCategoryListQuery, List<ProductCategoryDto>>
{
    public async Task<List<ProductCategoryDto>> Handle(
        GetProductCategoryListQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await repository.GetListAsync(cancellationToken);
        return categories
            .Select(x => new ProductCategoryDto
            {
                Id = x.Id,
                Uid = x.Uid,
                ProductCategoryId = x.ProductCategoryId,
                CategoryName = x.CategoryName ?? string.Empty,
                Description = x.Description
            })
            .ToList();
    }
}
