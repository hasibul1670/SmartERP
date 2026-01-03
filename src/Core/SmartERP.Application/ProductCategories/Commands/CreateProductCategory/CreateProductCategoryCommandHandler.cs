using MediatR;
using SmartERP.Application.ProductCategories.Repositories;
using SmartERP.Domain.Aggregates.ProductCategoryAggregate;
using SmartERP.Domain.Common;

namespace SmartERP.Application.ProductCategories.Commands.CreateProductCategory;

public sealed class CreateProductCategoryCommandHandler(IProductCategoryRepository repository)
    : IRequestHandler<CreateProductCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateProductCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await repository.ExistsByNameAsync(request.CategoryName, null, cancellationToken);
        if (exists)
            return Result<Guid>.Failure("Category name already exists.");

        var created = ProductCategory.Create(
            request.CategoryName,
            request.Description);

        if (!created.IsSuccess)
            return Result<Guid>.Failure(created.Errors);
        await repository.InsertAsync(created.Value, cancellationToken);
        return Result<Guid>.Success(created.Value.Uid);
    }
}
