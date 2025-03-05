using Ecommerce.Application.DTO.DiscountDto;
using Ecommerce.WebApi.DTO.DiscountApiDto;
using Ecommerce.WebApi.ValidationErrors;
using FluentValidation;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Ecommerce.WebApi.Validators.DiscountValidator
{
    public class AddDiscountDtoValidator : AbstractValidator<AddDiscountApiRequestDto>
    {
        public AddDiscountDtoValidator()
        {
            RuleFor(x => x.Description)
                    .NotEmpty().WithMessage(ValidationErrorMessages.DiscountDescCannotBeNullOrEmpty)
                    .Length(2, 500).WithMessage(ValidationErrorMessages.DiscountDescLength);

            RuleFor(x => x.Percentage)
               .Must(percentage => percentage.ToString(CultureInfo.InvariantCulture).All(char.IsDigit))
               .WithMessage(ValidationErrorMessages.PercentageInvalidFormat)
               .GreaterThan(0).WithMessage(ValidationErrorMessages.DiscountPercentageNotNegative);

            RuleFor(x => x.ProductId)
             .Must(stock => Regex.IsMatch(stock.ToString(), @"^\d+$"))//duhet rregulluar formati qe te pranoj vetem numra
              .WithMessage(ValidationErrorMessages.ProductIdInvalidFormat)
           .GreaterThan(0).WithMessage(ValidationErrorMessages.DiscountProductIdNotNegative);

            RuleFor(x => x.UserId)
            .Must(stock => Regex.IsMatch(stock.ToString(), @"^\d+$"))
             .WithMessage(ValidationErrorMessages.UserIdInvalidFormat)
          .GreaterThan(0).WithMessage(ValidationErrorMessages.DiscountUserIdNotNegative);
        }
    }
}
