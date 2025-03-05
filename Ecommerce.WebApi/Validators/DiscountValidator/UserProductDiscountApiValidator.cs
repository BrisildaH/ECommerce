using Ecommerce.WebApi.DTO.DiscountApiDto;
using Ecommerce.WebApi.ValidationErrors;
using FluentValidation;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Ecommerce.WebApi.Validators.DiscountValidator
{
    public class UserProductDiscountApiValidator : AbstractValidator<UserProductDiscountApi>
    {
        public UserProductDiscountApiValidator()
        {
            RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationErrorMessages.DiscountDescCannotBeNullOrEmpty)
            .Length(2, 500).WithMessage(ValidationErrorMessages.DiscountDescLength);

            RuleFor(x => x.Percentage)
           .Must(percentage => percentage.ToString(CultureInfo.InvariantCulture).All(char.IsDigit))
           .WithMessage(ValidationErrorMessages.PercentageInvalidFormat)
           .GreaterThan(0).WithMessage(ValidationErrorMessages.DiscountPercentageNotNegative);

            RuleFor(x => x.ProductId)
            .Must(productId => Regex.IsMatch(productId.ToString(), @"^\d+$"))
            .WithMessage(ValidationErrorMessages.ProductIdInvalidFormat)
            .GreaterThan(0).WithMessage(ValidationErrorMessages.DiscountProductIdNotNegative);

            RuleFor(x => x.UserId)
           .Must(userId => Regex.IsMatch(userId.ToString(), @"^\d+$"))
           .WithMessage(ValidationErrorMessages.UserIdInvalidFormat)
           .GreaterThan(0).WithMessage(ValidationErrorMessages.DiscountUserIdNotNegative);
        }
    }
}
