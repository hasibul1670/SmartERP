using MediatR;
using SmartERP.Domain.Common;

namespace SmartERP.Application.ProductCategories.Commands.CreateProductCategory;

public sealed record CreateProductCategoryCommand(
    string CategoryName,
    string? Description
) : IRequest<Result<Guid>>;