using MediatR;
using SmartERP.Application.ProductCategories.Repositories;
using SmartERP.Domain.Common;

namespace SmartERP.Application.ProductCategories.Commands.UpdateProductCategory;

public sealed class UpdateProductCategoryHandler(IProductCategoryRepository repository)
    : IRequestHandler<UpdateProductCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByUidAsync(request.Uid, cancellationToken);
        if (existing is null)
            return Result.Failure("not_found", "Product category not found.");

        var exists = await repository.ExistsByNameAsync(
            request.CategoryName,
            request.Uid,
            cancellationToken);
        if (exists)
            return Result.Failure("duplicate", "Category name already exists.");

        existing.CategoryName = request.CategoryName.Trim();
        existing.Description = request.Description?.Trim();

        var updated = await repository.UpdateAsync(existing, cancellationToken);
        return updated ? Result.Success() : Result.Failure("update_failed", "Failed to update product category.");
    }
}
