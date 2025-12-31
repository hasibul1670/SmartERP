using MediatR;
using SmartERP.Domain.Aggregates.ProductCategoryAggregate;
using SmartERP.Domain.Common;

namespace SmartERP.Application.ProductCategories.Commands.CreateProductCategory;

public sealed class CreateProductCategoryCommandHandler
    : IRequestHandler<CreateProductCategoryCommand, Result<int>>
{
    private readonly IProductCategoryRepository _repository;

    public CreateProductCategoryCommandHandler(IProductCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<int>> Handle(
        CreateProductCategoryCommand request,
        CancellationToken cancellationToken)
    {
        // 1️⃣ Domain creation (rules)
        var created = ProductCategory.Create(
            request.CategoryName,
            request.Description);

        if (!created.IsSuccess)
            // forward domain errors
            return Result<int>.Failure(created.Errors);

        // 2️⃣ Persist
        await _repository.InsertAsync(created.Value, cancellationToken);

        // 3️⃣ Return success (ID only, CleanHr style)
        return Result<int>.Success(created.Value.Id);
    }
}