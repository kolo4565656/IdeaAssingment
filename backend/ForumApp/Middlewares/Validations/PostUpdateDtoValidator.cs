using FluentValidation;
using ForumApplication.Dtos;

namespace ForumApi.Middlewares.Validations
{
    public class PostUpdateDtoValidator : AbstractValidator<PostUpdateDto>
    {
        public PostUpdateDtoValidator()
        {
            AddBasicRules();
        }
        private void AddBasicRules()
        {
            RuleFor(x => x.Name).NotEmpty().When(x => x.Name != null);
            RuleFor(x => x.Content).NotEmpty().When(x => x.Content != null);
            RuleFor(x => x.Description).NotEmpty().When(x => x.Description != null);
            RuleFor(x => x.Description).MaximumLength(200).When(x => x.Description != null);
        }
    }
}
