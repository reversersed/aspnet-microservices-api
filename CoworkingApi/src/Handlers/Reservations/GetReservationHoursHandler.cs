using CoworkingApi.src.Queries.Reservations;
using CoworkingApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Handlers.Reservations;

public class GetReservationHoursHandler(IReservationService _reservationService) : IRequestHandler<GetReservationHoursQuery, ActionResult<BaseResponse<bool[]>>>
{
    private readonly IReservationService reservationService = _reservationService ?? throw new ArgumentNullException(nameof(reservationService));
    public async Task<ActionResult<BaseResponse<bool[]>>> Handle(GetReservationHoursQuery request, CancellationToken cancellationToken)
    {
        var response = await reservationService.GetReservationHours(request.RoomId, request.Date) ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Не удается получить данные");
        return new BaseResponse<bool[]> { Code = ResponseCodes.DataFound, Message = $"Найдено {response.Count(x => x)} занятых часов", Data = response };
    }
}
