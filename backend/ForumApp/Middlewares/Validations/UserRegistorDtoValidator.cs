using FluentValidation;
using ForumApplication.Dtos;
using ForumPersistence.Entity.User;
using Microsoft.AspNetCore.Identity;

namespace ForumApi.Middlewares.Validations
{
    public class UserRegistorDtoValidator:AbstractValidator<UserRegisterDto>
    {
        public UserRegistorDtoValidator(UserManager<ApplicationUser> userManager)
        {
            AddBasicRules();
            AddExistRules(userManager);
        }

        private void AddExistRules(UserManager<ApplicationUser> userManager)
        {
            RuleFor(x => x.UserName).MustAsync(
                (dto, userName, context, cancellation) => IsNotExistUserName(userName, cancellation, userManager)
            ).When(x => !string.IsNullOrWhiteSpace(x.UserName))
            .WithMessage(string.Format(ValidationConstants.existedError, "{PropertyValue}"));

            RuleFor(x => x.Email).MustAsync(
                (dto, email, context, cancellation) => IsNotExistEmail(email, cancellation, userManager)
            ).When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage(string.Format(ValidationConstants.existedError, "{PropertyValue}"));
        }

        private void AddBasicRules()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyValue}"));

            RuleFor(x => x.Email).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(string.Format(ValidationConstants.invalidError, "{PropertyName}"));

            RuleFor(x => x.FirstName).NotEmpty().WithMessage(string.Format(ValidationConstants.invalidError, "First name"));
            RuleFor(x => x.LastName).NotEmpty().WithMessage(string.Format(ValidationConstants.invalidError, "Last name"));

            RuleFor(x => x.Role).NotNull().WithMessage(string.Format(ValidationConstants.requireError, "Role"));
            RuleFor(x => x.Role).IsInEnum().WithMessage(string.Format("{0} is a invalid enum for role", "{PropertyValue}"));
        }

        private async Task<bool> IsNotExistUserName(string userName, CancellationToken cancellation, UserManager<ApplicationUser> userManager)
        {
            cancellation.ThrowIfCancellationRequested();
            ApplicationUser user = await userManager.FindByNameAsync(userName);
            return user == null;
        }

        private async Task<bool> IsNotExistEmail(string email, CancellationToken cancellation, UserManager<ApplicationUser> userManager)
        {
            cancellation.ThrowIfCancellationRequested();
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            return user == null;
        }
    }
}
