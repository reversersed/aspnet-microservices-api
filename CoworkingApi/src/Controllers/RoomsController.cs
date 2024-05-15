using CoworkingApi.src.Queries.Reservations;
using CoworkingApi.src.Queries.Rooms;
using CoworkingApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Controllers;
[Route("[controller]")]
[ApiController]
public class RoomsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<RoomDTO>>> Get(long id)
    {
        return await _mediator.Send(new GetRoomByIdQuery { Id = id });
    }
    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<RoomDTO>>>> Get([FromQuery] string building, [FromQuery] int floor)
    {
        return await _mediator.Send(new GetRoomByFilterQuery { Building = building, Floor = floor });
    }
    [HttpPost]
    public async Task<ActionResult<BaseResponse<RoomDTO>>> Create([FromForm] AddRoomQuery query)
    {
        return await _mediator.Send(query);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse>> Delete(long id)
    {
        return await _mediator.Send(new DeleteRoomQuery { Id = id });
    }
    [HttpPatch("{id}")]
    public async Task<ActionResult<BaseResponse<RoomDTO>>> Patch(long id, [FromForm] UpdateRoomQuery query)
    {
        query.Id = id;
        return await _mediator.Send(query);
    }
    [HttpGet("{id}/reservations")]
    public async Task<ActionResult<BaseResponse<IEnumerable<ReservationDTO>>>> GetReservations(long id, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        return await mediator.Send(new GetReservationsByRoomQuery { RoomId = id, StartDate = startDate ?? DateTime.MinValue, EndDate = endDate ?? DateTime.MaxValue });
    }
    [HttpGet("{id}/reservationhours")]
    public async Task<ActionResult<BaseResponse<bool[]>>> GetReservationHours(long id, [FromQuery] DateOnly date)
    {
        return await mediator.Send(new GetReservationHoursQuery { RoomId = id, Date = date });
    }
    [HttpPost("{id}/reserve")]
    public async Task<ActionResult<BaseResponse>> ReserveRoom(long id, [FromBody] CreateReservationQuery query)
    {
        query.RoomId = id;
        return await mediator.Send(query);
    }
}