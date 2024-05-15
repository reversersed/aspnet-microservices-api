using FluentValidation;
using NewsApi.src.Queries.Comments;

namespace NewsApi.src.Validators.Comments;

public class DeleteCommentValidator : AbstractValidator<DeleteCommentQuery>
{
    public DeleteCommentValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Невалидный идентификатор комментария");
    }
}