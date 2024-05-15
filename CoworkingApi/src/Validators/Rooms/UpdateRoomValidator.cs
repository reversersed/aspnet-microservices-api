using CoworkingApi.src.Queries.Rooms;
using FluentValidation;

namespace CoworkingApi.src.Validators.Rooms;

public class UpdateRoomValidator : AbstractValidator<UpdateRoomQuery>
{
    public UpdateRoomValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Укажите валидный Id");
        var validExtensions = new string[] { ".png", ".jpg", ".jpeg" };
        RuleFor(x => x.RoomImage).Custom((x, c) => {
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
