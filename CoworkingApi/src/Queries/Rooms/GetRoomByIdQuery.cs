using CoworkingApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Queries.Rooms;

public class GetRoomByIdQuery : IRequest<ActionResult<BaseResponse<RoomDTO>>>
{
    public long Id { get; set; } = 0;
}
