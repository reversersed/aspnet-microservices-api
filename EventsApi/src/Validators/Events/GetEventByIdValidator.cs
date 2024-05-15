using EventsApi.src.Queries.Events;
using FluentValidation;

namespace EventsApi.src.Validators.Events;

public class GetEventByIdValidator : AbstractValidator<GetEventByIdQuery>
{
    public GetEventByIdValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Указан невалидный Id");
    }
}
