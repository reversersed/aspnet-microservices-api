using FluentValidation;
using NewsApi.src.Queries.Articles;

namespace NewsApi.src.Validators.Articles;

public class CreateArticleValidator : AbstractValidator<CreateArticleQuery>
{
    public CreateArticleValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Заголовок не должен быть пустым");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Текст статьи не должен быть пустым");
    }
}