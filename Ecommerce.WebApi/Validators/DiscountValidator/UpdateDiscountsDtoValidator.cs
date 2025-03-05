using Ecommerce.WebApi.DTO.DiscountApiDto;
using FluentValidation;

namespace Ecommerce.WebApi.Validators.DiscountValidator
{
    public class UpdateDiscountsDtoValidator : AbstractValidator<UpdateDiscountsApiRequestDto>
    {
        public UpdateDiscountsDtoValidator()
        {
            RuleForEach(x => x.Discounts)
                .SetValidator(new DiscountsApiValidator());
        }

    }
}