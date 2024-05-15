using CoworkingApi.src.Queries.Reservations;
using FluentValidation;

namespace CoworkingApi.src.Validators.Reservations;

public class GetReservationsByRoomValidator : AbstractValidator<GetReservationsByRoomQuery>
{
    public GetReservationsByRoomValidator()
    {
        RuleFor(x => x.RoomId).GreaterThan(0).WithMessage("Укажите валидный Id");
    }
}
