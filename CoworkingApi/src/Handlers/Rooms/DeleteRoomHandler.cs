using CoworkingApi.src.Queries.Rooms;
using CoworkingApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Handlers.Rooms;

public class DeleteRoomHandler(IRoomService roomService) : IRequestHandler<DeleteRoomQuery, ActionResult<BaseResponse>>
{
    private readonly IRoomService _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
    public async Task<ActionResult<BaseResponse>> Handle(DeleteRoomQuery request, CancellationToken cancellationToken)
    {
        await _roomService.Delete(request.Id);
        return new OkObjectResult(new BaseResponse { Code = ResponseCodes.DataDeleted, Message = "Комната удалена" });
    }
}
