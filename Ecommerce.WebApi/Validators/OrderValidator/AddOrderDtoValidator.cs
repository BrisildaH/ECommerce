using Ecommerce.WebApi.DTO.OrderApiDto;
using Ecommerce.WebApi.ValidationErrors;
using FluentValidation;

namespace Ecommerce.WebApi.Validators.OrderValidator
{
    public class AddOrderDtoValidator : AbstractValidator<AddOrderApiRequestDto>
    {
        public AddOrderDtoValidator()
        {
            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage(ValidationErrorMessages.OrderItemsNotEmpty); 

        RuleForEach(x => x.OrderItems)
            .ChildRules(item =>
            {
                item.RuleFor(o => o.ProductId)
                    .GreaterThan(0).WithMessage(ValidationErrorMessages.ProductIdPositiveNumber);

                item.RuleFor(o => o.Quantity)
                    .GreaterThan(0).WithMessage(ValidationErrorMessages.QuantityPositiveNumber);
            });
        }
    }
}



