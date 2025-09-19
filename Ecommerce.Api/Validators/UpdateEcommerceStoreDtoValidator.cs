using Ecommerce.Api.DTOs.EcommerceStore;
using FluentValidation;

namespace Ecommerce.Api.Validators
{
    public class UpdateEcommerceStoreDtoValidator : AbstractValidator<CreateEcommerceStoreDto>
    {
        public UpdateEcommerceStoreDtoValidator()
        {
            RuleFor(x => x.Product)
                .NotNull().WithMessage("Product name cannot be null.")
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Category)
                .NotNull().WithMessage("Category cannot be null.")
                .NotEmpty().WithMessage("Category is required.")
                .MaximumLength(500).WithMessage("Category must not exceed 500 characters.");

            RuleFor(x => x.Orders)
                .NotNull().WithMessage("Orders cannot be null.")
                .NotEmpty().WithMessage("Orders is required.");
        }   
    }
}
