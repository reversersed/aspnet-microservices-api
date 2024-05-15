using FluentValidation;
using NewsApi.src.Queries.Comments;

namespace NewsApi.src.Validators.Comments;

public class FindCommentByIdValidator : AbstractValidator<FindCommentByIdQuery>
{
    public FindCommentByIdValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Невалидный идентификатор комментария");
    }
}