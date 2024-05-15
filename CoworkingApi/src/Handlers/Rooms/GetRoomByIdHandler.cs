using CoworkingApi.src.Queries.Rooms;
using CoworkingApi.src.Services.Entities;
using CoworkingApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Handlers.Rooms;

public class GetRoomByIdHandler(IRoomService roomService) : IRequestHandler<GetRoomByIdQuery, ActionResult<BaseResponse<RoomDTO>>>
{
    private readonly IRoomService _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
    public async Task<ActionResult<BaseResponse<RoomDTO>>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _roomService.Get(request.Id);
        return new OkObjectResult(new BaseResponse<RoomDTO> { Code = ResponseCodes.DataFound, Message = "Комната найдена", Data = response });
    }
}