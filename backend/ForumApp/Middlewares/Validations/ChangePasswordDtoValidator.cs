using FluentValidation;
using ForumApplication.Dtos;

namespace ForumApi.Middlewares.Validations
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            AddBasicRules();
        }
        private void AddBasicRules()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
        }
    }
}
