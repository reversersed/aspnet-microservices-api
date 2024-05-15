using FluentValidation;
using NewsApi.src.Queries.Articles;

namespace NewsApi.src.Validators.Articles;

public class PatchArticleValidator : AbstractValidator<PatchArticleQuery>
{
    public PatchArticleValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Невалидный идентификатор статьи");
        RuleFor(x => x.Title).NotEmpty().When(x => string.IsNullOrWhiteSpace(x.Content)).WithMessage("Вы должны указать текст или заголовок статьи");
        RuleFor(x => x.Content).NotEmpty().When(x => string.IsNullOrWhiteSpace(x.Title)).WithMessage("Вы должны указать текст или заголовок статьи");
    }
}