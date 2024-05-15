using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Queries.Reservations;

public class CreateReservationQuery : IRequest<ActionResult<BaseResponse>>
{
    public long RoomId { get; set; } = 0;
    public DateTime ReservationStart { get; set; } = DateTime.MinValue;
    public DateTime ReservationEnd { get; set; } = DateTime.MinValue;
    public string Purpose { get; set; } = string.Empty;
}
