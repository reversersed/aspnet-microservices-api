using EventsApi.src.Queries.Categories;
using FluentValidation;

namespace EventsApi.src.Validators.Categories
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryQuery>
    {
        public DeleteCategoryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Укажите валидный Id");
        }
    }
}
