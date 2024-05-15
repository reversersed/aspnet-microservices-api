using EventsApi.src.Queries.Comments;
using FluentValidation;

namespace EventsApi.src.Validators.Comments;

public class AddCommentValidator : AbstractValidator<AddCommentQuery>
{
    public AddCommentValidator()
    {
        RuleFor(x => x.EventId).GreaterThan(0).WithMessage("Укажите валидное мероприятие");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Введите текст комментария");
    }
}
