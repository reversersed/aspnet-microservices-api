using FluentValidation;
using NewsApi.src.Queries.Comments;

namespace NewsApi.src.Validators.Comments;

public class CreateCommentValidator : AbstractValidator<CreateCommentQuery>
{
    public CreateCommentValidator()
    {
        RuleFor(x => x.ArticleId).GreaterThan(0).WithMessage("Невалидный идентификатор комментария");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Текст комментария не должен быть пустым");
    }
}