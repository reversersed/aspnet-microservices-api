using FluentValidation;
using NewsApi.src.Queries.Articles;

namespace NewsApi.src.Validators.Articles;

public class DeleteArticleValidator : AbstractValidator<DeleteArticleQuery>
{
    public DeleteArticleValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Требуется указать идентификатор")
            .GreaterThan(0).WithMessage("Невалидный идентификатор статьи");
    }
}