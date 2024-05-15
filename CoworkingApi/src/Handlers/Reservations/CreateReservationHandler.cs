using CoworkingApi.src.Queries.Reservations;
using CoworkingApi.src.Services.Entities;
using CoworkingApi.src.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Handlers.Reservations;

public class CreateReservationHandler(IReservationService _reservationService, IHttpContextAccessor _httpContextAccessor) : IRequestHandler<CreateReservationQuery, ActionResult<BaseResponse>>
{
    private readonly IReservationService reservationService = _reservationService ?? throw new ArgumentNullException(nameof(_reservationService));
    private readonly IHttpContextAccessor httpContextAccessor = _httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));

    public async Task<ActionResult<BaseResponse>> Handle(CreateReservationQuery request, CancellationToken cancellationToken)
    {
        var reservation = request.Adapt<ReservationDTO>();
        reservation.Holder = httpContextAccessor.HttpContext?.User.Identity?.Name ?? throw new CustomExceptionResponse(ResponseCodes.Unauthorized, "Не удается получить имя пользователя");
        await reservationService.CreateReservation(reservation, request.RoomId);

        return new CreatedAtActionResult(null,null,null, new BaseResponse { Code = ResponseCodes.DataCreated, Message = "Комната успешно забронирована" });
    }
}
