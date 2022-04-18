using FluentValidation;
using ForumApplication.Dtos;

namespace ForumApi.Middlewares.Validations
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            AddBasicRules();
        }
        private void AddBasicRules()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Password).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Password).Length(0, 10).WithMessage(
                string.Format(ValidationConstants.rangeError, "{PropertyName}", "{MaxLength}", "{MinLength}")
            );
        }
    }
}
