using Ecommerce.Application.DTO;
using Ecommerce.WebApi.DTO.ProductApiDto;
using Ecommerce.WebApi.ValidationErrors;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Ecommerce.WebApi.Validators.ProductValidator
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductApiRequestDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage(ValidationErrorMessages.ProductNameCannotBeNullOrEmpty)
               .Length(2, 100).WithMessage(ValidationErrorMessages.ProductNameLength);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(ValidationErrorMessages.ProductDescCannotBeNullOrEmpty)
                .Length(2, 500).WithMessage(ValidationErrorMessages.ProductDescLength);

            RuleFor(x => x.Price)
               .Must(price => Regex.IsMatch(price.ToString(), @"^\d+(\.\d{1,2})?$")) //per t'u pare serish...
                .WithMessage(ValidationErrorMessages.ProductPriceNotValidFormat)
               .GreaterThan(0)
               .WithMessage(ValidationErrorMessages.ProductPriceNotNegative);

            RuleFor(x => x.IsPublic)
                .NotNull().WithMessage(ValidationErrorMessages.ProductIsPublic);

            RuleFor(x => x.IsAvailable)
                .NotNull().WithMessage(ValidationErrorMessages.ProductIsAvailable);

            RuleFor(x => x.Stock)
               .Must(stock => Regex.IsMatch(stock.ToString(), @"^\d+$"))
               .WithMessage(ValidationErrorMessages.StockInvalidFormat)
               .GreaterThanOrEqualTo(0).WithMessage(ValidationErrorMessages.StockNotNegative);
        }
    }
}
