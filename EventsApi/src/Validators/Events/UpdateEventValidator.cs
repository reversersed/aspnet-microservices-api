using EventsApi.src.Queries.Events;
using FluentValidation;

namespace EventsApi.src.Validators.Events;

public class UpdateEventValidator : AbstractValidator<UpdateEventQuery>
{
    public UpdateEventValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).When(x => x.Location is not null).WithMessage("Мероприятие не найдено");
        RuleFor(x => x.CategoryId).GreaterThan(0).When(x => x.Location is not null).WithMessage("Категория не найдена");
        RuleFor(x => x.Seats).GreaterThanOrEqualTo(0).When(x => x.Location is not null).WithMessage("Укажите валидное количество мест");
        RuleFor(x => x.Title).NotEmpty().When(x => x.Location is not null).WithMessage("Укажите название мероприятия");
        RuleFor(x => x.StartDate).GreaterThan(DateTime.UtcNow).When(x => x.Location is not null).WithMessage("Дата начала не может быть меньше текущей");
        RuleFor(x => x.CategoryId).GreaterThan(0).When(x => x.Location is not null).WithMessage("Укажите валидную категорию");
        RuleFor(x => x.Location).NotEmpty().When(x => x.Location is not null).WithMessage("Укажите место проведения");

        var validExtensions = new string[] { ".png", ".jpg", ".jpeg" };
        RuleFor(x => x.Image).Custom((x, c) => {
            if (x is not null)
            {
                if (x.Length > 1024 * 1024 * 5)
                    c.AddFailure(new FluentValidation.Results.ValidationFailure { AttemptedValue = x.Length, ErrorMessage = "Размер файла не должен превышать 5 Мб" });
                if (!validExtensions.Contains(Path.GetExtension(x.FileName)))
                    c.AddFailure(new FluentValidation.Results.ValidationFailure { AttemptedValue = x.FileName, ErrorMessage = "Выбранный файл не поддерживается. Доступные форматы: *" + string.Join(" | *", validExtensions) });
            }
        });
    }
}
