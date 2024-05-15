using CoworkingApi.src.Queries.Reservations;
using FluentValidation;

namespace CoworkingApi.src.Validators.Reservations;

public class CreateReservationValidator : AbstractValidator<CreateReservationQuery>
{
    public CreateReservationValidator()
    {
        RuleFor(x => x.ReservationStart).GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Начальное время не может быть меньше текущего");
        RuleFor(x => x.ReservationEnd).GreaterThan(x => x.ReservationStart).WithMessage("Конечное время должно быть больше начального");
        RuleFor(x => x.RoomId).GreaterThan(0).WithMessage("Укажите валидный id комнаты");
        RuleFor(x => x.Purpose).NotEmpty().WithMessage("Укажите цель бронирования");
    }
}