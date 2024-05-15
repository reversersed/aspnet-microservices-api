using EventsApi.src.Queries.Comments;
using FluentValidation;

namespace EventsApi.src.Validators.Comments;

public class DeleteCommentValidator : AbstractValidator<DeleteCommentQuery>
{
    public DeleteCommentValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Укажите существующий комментарий");
    }
}
