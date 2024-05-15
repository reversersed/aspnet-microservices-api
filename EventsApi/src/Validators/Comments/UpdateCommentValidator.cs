using EventsApi.src.Queries.Comments;
using FluentValidation;

namespace EventsApi.src.Validators.Comments;

public class UpdateCommentValidator : AbstractValidator<UpdateCommentQuery>
{
    public UpdateCommentValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Укажите существующий комментарий");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Введите текст комментария");
    }
}
