using MediatR;
using SmartERP.Application.ProductCategories.Repositories;
using SmartERP.Domain.Common;

namespace SmartERP.Application.ProductCategories.Commands.DeleteProductCategory;

public sealed class DeleteProductCategoryHandler(IProductCategoryRepository repository)
    : IRequestHandler<DeleteProductCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var deleted = await repository.DeleteAsync(request.Uid, cancellationToken);
        return deleted
            ? Result.Success()
            : Result.Failure("not_found", "Product category not found.");
    }
}
