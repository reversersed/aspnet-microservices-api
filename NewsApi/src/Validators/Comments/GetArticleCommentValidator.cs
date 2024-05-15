using FluentValidation;
using NewsApi.src.Queries.Comments;

namespace NewsApi.src.Validators.Comments;

public class GetArticleCommentValidator : AbstractValidator<GetArticleCommentQuery>
{
    public GetArticleCommentValidator()
    {
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0).WithMessage("Смещение не может быть меньше 0");
        RuleFor(x => x.Count).NotEmpty().WithMessage("Требуется указать количество комментариев")
            .GreaterThan(0).WithMessage("Количество комментариев не может быть меньше 1");
    }
}
