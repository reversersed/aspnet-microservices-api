using CoworkingApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Queries.Reservations;

public class GetReservationsByRoomQuery : IRequest<ActionResult<BaseResponse<IEnumerable<ReservationDTO>>>>
{
    public long RoomId { get; set; } = 0;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
