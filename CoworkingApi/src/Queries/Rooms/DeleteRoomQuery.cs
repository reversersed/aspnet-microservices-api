using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Queries.Rooms;

public class DeleteRoomQuery : IRequest<ActionResult<BaseResponse>>
{
    public long Id { get; set; } = 0;
}
