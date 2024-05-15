using FluentValidation;
using NewsApi.src.Queries.Articles;

namespace NewsApi.src.Validators.Articles;

public class FindArticleByIdValidator : AbstractValidator<FindArticleByIdQuery>
{
    public FindArticleByIdValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Невалидный идентификатор статьи");
    }
}