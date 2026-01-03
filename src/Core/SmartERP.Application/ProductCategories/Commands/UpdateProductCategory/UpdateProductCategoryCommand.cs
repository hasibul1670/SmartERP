using MediatR;
using SmartERP.Domain.Common;

namespace SmartERP.Application.ProductCategories.Commands.UpdateProductCategory;

public sealed record UpdateProductCategoryCommand(
    Guid Uid,
    string CategoryName,
    string? Description) : IRequest<Result>;