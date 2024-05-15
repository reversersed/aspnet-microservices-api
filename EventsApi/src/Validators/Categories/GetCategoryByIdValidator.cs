using EventsApi.src.Queries.Categories;
using FluentValidation;

namespace EventsApi.src.Validators.Categories;

public class GetCategoryByIdValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Указан неверный Id категории");
    }
}
