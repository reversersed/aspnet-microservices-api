using CoworkingApi.src.Queries.Rooms;
using FluentValidation;

namespace CoworkingApi.src.Validators.Rooms;

public class GetRoomByIdValidator : AbstractValidator<GetRoomByIdQuery>
{
    public GetRoomByIdValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Укажите валидный Id");
    }
}
