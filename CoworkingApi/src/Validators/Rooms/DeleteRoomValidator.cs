using CoworkingApi.src.Queries.Rooms;
using FluentValidation;

namespace CoworkingApi.src.Validators.Rooms;

public class DeleteRoomValidator : AbstractValidator<DeleteRoomQuery>
{
    public DeleteRoomValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Укажите валидный Id");
    }
}
