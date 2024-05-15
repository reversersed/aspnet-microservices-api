using CoworkingApi.src.Queries.Reservations;
using CoworkingApi.src.Services.Entities;
using CoworkingApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Handlers.Reservations;

public class GetReservationsByRoomHandler(IReservationService _reservationService) : IRequestHandler<GetReservationsByRoomQuery, ActionResult<BaseResponse<IEnumerable<ReservationDTO>>>>
{
    private readonly IReservationService reservationService = _reservationService ?? throw new ArgumentNullException(nameof(reservationService));
    public async Task<ActionResult<BaseResponse<IEnumerable<ReservationDTO>>>> Handle(GetReservationsByRoomQuery request, CancellationToken cancellationToken)
    {
        var response = await reservationService.Get(request.RoomId, request.StartDate, request.EndDate);
        return new OkObjectResult(new BaseResponse<IEnumerable<ReservationDTO>> { Code = ResponseCodes.DataFound, Message = $"Найдено {response.Count()} записей", Data = response });
    }
}
