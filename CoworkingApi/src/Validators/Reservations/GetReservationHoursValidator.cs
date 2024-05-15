using CoworkingApi.src.Queries.Reservations;
using FluentValidation;

namespace CoworkingApi.src.Validators.Reservations;

public class GetReservationHoursValidator : AbstractValidator<GetReservationHoursQuery>
{
    public GetReservationHoursValidator()
    {
        RuleFor(x => x.RoomId).GreaterThan(0).WithMessage("Укажите валидный id комнаты");
    }
}
