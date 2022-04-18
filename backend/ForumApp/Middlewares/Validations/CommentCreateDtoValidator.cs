using FluentValidation;
using ForumApplication.Dtos;

namespace ForumApi.Middlewares.Validations
{
    public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
    {
        public CommentCreateDtoValidator()
        {
            AddBasicRules();
        }
        private void AddBasicRules()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Content).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Rating).NotEmpty().WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Rating).GreaterThanOrEqualTo(1)
                .WithMessage(string.Format(ValidationConstants.minError, "{PropertyName}", 1));
            RuleFor(x => x.Rating).LessThanOrEqualTo(5)
                .WithMessage(string.Format(ValidationConstants.maxError, "{PropertyName}", 5));
        }
    }
}
