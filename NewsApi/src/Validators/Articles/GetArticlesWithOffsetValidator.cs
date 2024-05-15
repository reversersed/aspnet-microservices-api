using FluentValidation;
using NewsApi.src.Queries.Articles;

namespace NewsApi.src.Validators.Articles;

public class GetArticlesWithOffsetValidator : AbstractValidator<GetArticlesWithOffsetQuery>
{
    public GetArticlesWithOffsetValidator()
    {
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0).WithMessage("Смещение не может быть меньше 0");
        RuleFor(x => x.Count).GreaterThanOrEqualTo(1).WithMessage("Количество статей не может быть меньше 1");
    }
}