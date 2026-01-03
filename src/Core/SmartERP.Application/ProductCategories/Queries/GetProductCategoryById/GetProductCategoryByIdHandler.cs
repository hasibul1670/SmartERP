using MediatR;
using SmartERP.Application.ProductCategories.Dtos;
using SmartERP.Application.ProductCategories.Repositories;

namespace SmartERP.Application.ProductCategories.Queries.GetProductCategoryById;

public sealed class GetProductCategoryByIdHandler(IProductCategoryRepository repository)
    : IRequestHandler<GetProductCategoryByIdQuery, ProductCategoryDto?>
{
    public async Task<ProductCategoryDto?> Handle(
        GetProductCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await repository.GetByUidAsync(request.Uid, cancellationToken);
        if (category is null) return null;

        return new ProductCategoryDto
        {
            Id = category.Id,
            Uid = category.Uid,
            ProductCategoryId = category.ProductCategoryId,
            CategoryName = category.CategoryName ?? string.Empty,
            Description = category.Description
        };
    }
}
