using EventsApi.src.Queries.Events;
using FluentValidation;

namespace EventsApi.src.Validators.Events;

public class DeleteEventValidator : AbstractValidator<DeleteEventQuery>
{
    public DeleteEventValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Укажите валидный Id");
    }
}
