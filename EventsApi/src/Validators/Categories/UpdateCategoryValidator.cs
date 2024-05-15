using EventsApi.src.Queries.Categories;
using FluentValidation;

namespace EventsApi.src.Validators.Categories
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryQuery>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Указан неправильный Id");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Укажите название категории");
        }
    }
}
