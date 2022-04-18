using FluentValidation;
using ForumApplication.Dtos;

namespace ForumApi.Middlewares.Validations
{
    public class PostCreateDtoValidator : AbstractValidator<PostCreateDto>
    {
        public PostCreateDtoValidator()
        {
            AddBasicRules();
        }
        private void AddBasicRules()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Content).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Description).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Description).MaximumLength(200).WithMessage(string.Format(ValidationConstants.maxError, "{PropertyName} length", 200));
        }
    }
}
