using EventsApi.src.Queries.Categories;
using FluentValidation;

namespace EventsApi.src.Validators.Categories
{
    public class AddCategoryValidator : AbstractValidator<AddCategoryQuery>
    {
        public AddCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Введите название категории");
        }
    }
}
