using EventsApi.src.Queries.Categories;
using FluentValidation;

namespace EventsApi.src.Validators.Categories;

public class GetCategoriesValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesValidator()
    {
        RuleFor(x => x.Filter).Must((x) => x.Equals("all") || x.Equals("notempty")).WithMessage("Указаны неправильные фильтры. Доступные значения: all | notempty");
    }
}
