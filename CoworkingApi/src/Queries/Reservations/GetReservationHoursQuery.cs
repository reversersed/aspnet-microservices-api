using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Queries.Reservations;

public class GetReservationHoursQuery : IRequest<ActionResult<BaseResponse<bool[]>>>
{
    public long RoomId { get; set; } = 0;
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.Date);
}
