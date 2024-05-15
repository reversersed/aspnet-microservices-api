using CoworkingApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Queries.Rooms;

public class GetRoomByFilterQuery : IRequest<ActionResult<BaseResponse<IEnumerable<RoomDTO>>>>
{
    public string Building { get; set; } = "";
    public int Floor { get; set; } = 0;
}
