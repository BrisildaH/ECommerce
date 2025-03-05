using Ecommerce.WebApi.DTO.UserApiDto;
using Ecommerce.WebApi.ValidationErrors;
using FluentValidation;

namespace Ecommerce.WebApi.Validators.UserValidator
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserApiRequestDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.Username)
              .NotEmpty().WithMessage(ValidationErrorMessages.UsernameCannotBeNullOrEmpty);

            RuleFor(x => x.Password)
             .NotEmpty().WithMessage(ValidationErrorMessages.PasswordCannotBeNullOrEmpty);

            RuleFor(x => x.RoleId)
          .GreaterThan(0).WithMessage(ValidationErrorMessages.RoleIdCannotBeNullOrEmpty)
          .WithMessage(ValidationErrorMessages.RoleIdCannotBeNegativeOrZero);
        }
    }
}