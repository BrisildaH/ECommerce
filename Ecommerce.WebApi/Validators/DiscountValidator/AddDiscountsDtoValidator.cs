using Ecommerce.WebApi.DTO.DiscountApiDto;
using Ecommerce.WebApi.ValidationErrors;
using FluentValidation;

namespace Ecommerce.WebApi.Validators.DiscountValidator
{
    public class AddDiscountsDtoValidator : AbstractValidator<AddDiscountsApiRequestDto>
    {
        public AddDiscountsDtoValidator()
        {
            RuleForEach(x => x.Discounts)
                .SetValidator(new UserProductDiscountApiValidator()); 
        }
    }
}
