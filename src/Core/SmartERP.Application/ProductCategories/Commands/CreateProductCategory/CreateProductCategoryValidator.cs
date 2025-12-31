using FluentValidation;

namespace SmartERP.Application.ProductCategories.Commands.CreateProductCategory;

public class CreateProductCategoryValidator : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryValidator()
    {
        RuleFor(x => x.CategoryName).NotNull()
            .NotEmpty().WithMessage("CategoryName is required.")
            .MaximumLength(100).WithMessage("CategoryName must not exceed 100 characters.");
        RuleFor(x => x.Description).NotNull()
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");
    }
}