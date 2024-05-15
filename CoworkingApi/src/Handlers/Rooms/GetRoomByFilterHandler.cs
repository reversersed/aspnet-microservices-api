using CoworkingApi.src.Queries.Rooms;
using CoworkingApi.src.Services;
using CoworkingApi.src.Services.Entities;
using CoworkingApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Handlers.Rooms;

public class GetRoomByFilterHandler(IRoomService roomService) : IRequestHandler<GetRoomByFilterQuery, ActionResult<BaseResponse<IEnumerable<RoomDTO>>>>
{
    private readonly IRoomService _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
    public async Task<ActionResult<BaseResponse<IEnumerable<RoomDTO>>>> Handle(GetRoomByFilterQuery request, CancellationToken cancellationToken)
    {
        var response = await _roomService.Get(request.Building, request.Floor);
        return new OkObjectResult(new BaseResponse<IEnumerable<RoomDTO>> { Code = ResponseCodes.DataFound, Message = $"Найдено {response.Count()} комнат", Data= response });
    }
}
