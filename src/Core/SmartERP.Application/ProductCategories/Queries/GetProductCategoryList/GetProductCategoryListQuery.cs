using MediatR;
using SmartERP.Application.ProductCategories.Dtos;

namespace SmartERP.Application.ProductCategories.Queries.GetProductCategoryList;

public sealed record GetProductCategoryListQuery : IRequest<List<ProductCategoryDto>>;
