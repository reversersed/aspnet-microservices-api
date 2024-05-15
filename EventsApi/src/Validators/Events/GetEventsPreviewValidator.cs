using EventsApi.src.Queries.Events;
using FluentValidation;

namespace EventsApi.src.Validators.Events;

public class GetEventsPreviewValidator : AbstractValidator<GetEventsPreviewQuery>
{
    public GetEventsPreviewValidator()
    {
        RuleFor(x => x.Sorting).Must(x => x.Equals("nearest") || x.Equals("newest")).WithMessage("Невалидный фильтр");
        RuleFor(x => x.CategoryId).GreaterThanOrEqualTo(0).WithMessage("Выберите валидную категорию");
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0).WithMessage("Смещение должно быть не меньше 0");
        RuleFor(x => x.Count).GreaterThan(0).WithMessage("Количество мероприятий должно быть больше 0");
    }
}
