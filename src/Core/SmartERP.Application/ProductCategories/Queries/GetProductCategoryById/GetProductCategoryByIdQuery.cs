using MediatR;
using SmartERP.Application.ProductCategories.Dtos;

namespace SmartERP.Application.ProductCategories.Queries.GetProductCategoryById;

public sealed record GetProductCategoryByIdQuery(Guid Uid) : IRequest<ProductCategoryDto?>;