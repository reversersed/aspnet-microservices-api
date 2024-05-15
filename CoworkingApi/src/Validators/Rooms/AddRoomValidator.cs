using CoworkingApi.src.Queries.Rooms;
using FluentValidation;

namespace CoworkingApi.src.Validators.Rooms;

public class AddRoomValidator : AbstractValidator<AddRoomQuery>
{
    public AddRoomValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Укажите название комнаты");
        RuleFor(x => x.Seats).GreaterThan(0).WithMessage("Количество мест не может быть меньше нуля");
        RuleFor(x => x.Floor).NotNull().WithMessage("Укажите этаж комнаты");
        RuleFor(x => x.Building).NotEmpty().WithMessage("Укажите корпус");
        RuleFor(x => x.RoomNumber).NotEmpty().WithMessage("Укажите номер комнаты");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Укажите описание комнаты");

        var validExtensions = new string[] { ".png", ".jpg", ".jpeg" };
        RuleFor(x => x.RoomImage).Custom((x, c) => {
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
