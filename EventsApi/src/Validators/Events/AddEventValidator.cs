using EventsApi.src.Queries.Events;
using FluentValidation;

namespace EventsApi.src.Validators.Events;

public class AddEventValidator : AbstractValidator<AddEventQuery>
{
    public AddEventValidator()
    {
        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Выберите категорию");
        RuleFor(x => x.StartDate).GreaterThan(DateTime.UtcNow).WithMessage("Дата начала должна быть в будущем");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Укажите заголовок мероприятия");
        RuleFor(x => x.Seats).GreaterThanOrEqualTo(0).WithMessage("Количество мест должно быть валидно");
        RuleFor(x => x.Location).NotEmpty().WithMessage("Укажите место проведения");
        var validExtensions = new string[] { ".png", ".jpg", ".jpeg" };
        RuleFor(x => x.Image).Custom((x, c) => {
            if (x is null)
                c.AddFailure(new FluentValidation.Results.ValidationFailure { AttemptedValue = x, ErrorMessage = "Выберите изображение" });
            else
            {
                if (x.Length > 1024 * 1024 * 5)
                    c.AddFailure(new FluentValidation.Results.ValidationFailure { AttemptedValue = x.Length, ErrorMessage = "Размер файла не должен превышать 5 Мб" });
                if (!validExtensions.Contains(Path.GetExtension(x.FileName)))
                    c.AddFailure(new FluentValidation.Results.ValidationFailure { AttemptedValue = x.FileName, ErrorMessage = "Выбранный файл не поддерживается. Доступные форматы: *" + string.Join(" | *", validExtensions) });
            }
        });
    }
}
