using FluentValidation;

namespace SmartERP.Application.ProductCategories.Commands.UpdateProductCategory;

public class UpdateProductCategoryValidator : AbstractValidator<UpdateProductCategoryCommand>
{
    public UpdateProductCategoryValidator()
    {
        RuleFor(x => x.Uid)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.CategoryName).NotNull()
            .NotEmpty().WithMessage("CategoryName is required.")
            .MaximumLength(100).WithMessage("CategoryName must not exceed 100 characters.");

        RuleFor(x => x.Description).NotNull()
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");
    }
}
