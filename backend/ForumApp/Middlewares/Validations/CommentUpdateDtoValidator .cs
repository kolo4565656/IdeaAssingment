using FluentValidation;
using ForumApplication.Dtos;

namespace ForumApi.Middlewares.Validations
{
    public class CommentUpdateDtoValidator : AbstractValidator<CommentUpdateDto>
    {
        public CommentUpdateDtoValidator()
        {
            AddBasicRules();
        }
        private void AddBasicRules()
        {
            RuleFor(x => x.Name).NotEmpty().When(x => x.Name != null)
                .WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Content).NotEmpty().When(x => x.Content != null)
                .WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Rating).NotEmpty().When(x => x.Rating != null)
                .WithMessage(string.Format(ValidationConstants.requireError, "{PropertyName}"));
            RuleFor(x => x.Rating).GreaterThanOrEqualTo(1).When(x => x.Rating != null)
                .WithMessage(string.Format(ValidationConstants.minError, "{PropertyName}", 1));
            RuleFor(x => x.Rating).LessThanOrEqualTo(5).When(x => x.Rating != null)
                .WithMessage(string.Format(ValidationConstants.maxError, "{PropertyName}", 5));
        }
    }
}
