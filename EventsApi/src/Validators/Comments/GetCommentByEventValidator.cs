using EventsApi.src.Queries.Comments;
using FluentValidation;

namespace EventsApi.src.Validators.Comments;

public class GetCommentByEventValidator : AbstractValidator<GetCommentByEventQuery>
{
    public GetCommentByEventValidator()
    {
        RuleFor(x => x.EventId).GreaterThan(0).WithMessage("Укажите существующее мероприятие");
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0).WithMessage("Смещение не может быть меньше 0");
        RuleFor(x => x.Count).GreaterThan(0).WithMessage("Количество должно быть больше 0");
    }
}
