using MediatR;
using SmartERP.Domain.Common;

namespace SmartERP.Application.ProductCategories.Commands.DeleteProductCategory;

public sealed record DeleteProductCategoryCommand(Guid Uid) : IRequest<Result>;