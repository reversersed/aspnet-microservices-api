using EventsApi.src.Queries.Comments;
using FluentValidation;

namespace EventsApi.src.Validators.Comments;

public class GetCommentByIdValidator : AbstractValidator<GetCommentByIdQuery>
{
    public GetCommentByIdValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Укажите валидный Id");
    }
}
