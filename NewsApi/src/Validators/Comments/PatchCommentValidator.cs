using FluentValidation;
using NewsApi.src.Queries.Comments;

namespace NewsApi.src.Validators.Comments;

public class PatchCommentValidator : AbstractValidator<PatchCommentQuery>
{
    public PatchCommentValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Невалидный идентификатор комментария");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Вы должны указать текст комментария");
    }
}