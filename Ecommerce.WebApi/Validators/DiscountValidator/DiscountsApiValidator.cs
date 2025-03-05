using Ecommerce.Application.Resources;
using Ecommerce.WebApi.DTO.DiscountApiDto;
using Ecommerce.WebApi.ValidationErrors;
using FluentValidation;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Ecommerce.WebApi.Validators.DiscountValidator
{
    public class DiscountsApiValidator : AbstractValidator<DiscountsApi>
    {
        private static HashSet<int> _encounteredIds = new HashSet<int>();
        public DiscountsApiValidator()
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

            RuleFor(x => x.Id)
          .Must(userId => Regex.IsMatch(userId.ToString(), @"^\d+$"))
          .WithMessage(ValidationErrorMessages.DiscountIdInvalidFormat)
          .GreaterThan(0).WithMessage(ValidationErrorMessages.DiscountUIdNotNegative);

            RuleFor(x => x.Id)
                .Must(IsDistinct)
                  .WithMessage(ValidationErrorMessages.DiscountIdUnique);
        }
        public bool IsDistinct(int id)
        {
            if (_encounteredIds.Contains(id))
            {
                return false; 
            }
            else
            {
                _encounteredIds.Add(id); 
                return true; 
            }
        }
    }
}