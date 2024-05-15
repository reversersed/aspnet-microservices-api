using CoworkingApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Queries.Rooms;

public class AddRoomQuery : IRequest<ActionResult<BaseResponse<RoomDTO>>>
{
    public string Name { get; set; } = "";
    public string Building { get; set; } = "";
    public int? Floor { get; set; } = null;
    public string RoomNumber { get; set; } = "";
    public string Description { get; set; } = "";
    public int Seats { get; set; } = 0;
    public IFormFile? RoomImage { get; set; } = null;
}
