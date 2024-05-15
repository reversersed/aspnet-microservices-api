using CoworkingApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Queries.Rooms;

public class UpdateRoomQuery : IRequest<ActionResult<BaseResponse<RoomDTO>>>
{
    public long Id { get; set; } = 0;
    public string? Name { get; set; } = null;
    public string? Building { get; set; } = null;
    public int? Floor { get; set; } = null;
    public string? RoomNumber { get; set; } = null;
    public int? Seats { get; set; } = null;
    public string? Description { get; set; } = null;
    public IFormFile? RoomImage { get; set; } = null;
}
