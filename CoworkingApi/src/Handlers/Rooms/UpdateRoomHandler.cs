using System.Runtime.InteropServices;
using System;
using CoworkingApi.src.Queries.Rooms;
using CoworkingApi.src.Services.Entities;
using CoworkingApi.src.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;
using CoworkingApi.src.Data.Entities;

namespace CoworkingApi.src.Handlers.Rooms;

public class UpdateRoomHandler(IRoomService roomService, IWebHostEnvironment _environment, ILogger<UpdateRoomHandler> _logger) : IRequestHandler<UpdateRoomQuery, ActionResult<BaseResponse<RoomDTO>>>
{
    private readonly IRoomService _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
    private readonly IWebHostEnvironment environment = _environment ?? throw new ArgumentNullException(nameof(environment));
    private readonly ILogger<UpdateRoomHandler> logger = _logger ?? throw new ArgumentNullException(nameof(logger));
    public async Task<ActionResult<BaseResponse<RoomDTO>>> Handle(UpdateRoomQuery request, CancellationToken cancellationToken)
    {
        var room = await _roomService.Get(request.Id);
        var cfg = TypeAdapterConfig.GlobalSettings.Clone();
        cfg.Default.IgnoreNullValues(true);
        room = request.Adapt(room, cfg);

        if(request.RoomImage is not null)
        {
            var filePath = Path.Combine(environment.ContentRootPath, "files", CreateUniqueFileName(request.RoomImage));

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "");
            else
                Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "", UnixFileMode.OtherRead | UnixFileMode.UserWrite);

            room.RoomImage = filePath;

            await request.RoomImage.CopyToAsync(new FileStream(filePath, FileMode.Create), cancellationToken);
        }
        logger.LogInformation("{file} is null: {is}", request.RoomImage, request.RoomNumber is null);
        var entity = await _roomService.Update(room, request.RoomImage is not null);

        return new OkObjectResult(new BaseResponse<RoomDTO> { Code = ResponseCodes.DataUpdated, Message = "Комната обновлена", Data = entity });
    }
    private static string CreateUniqueFileName(IFormFile file)
    {
        var filename = Path.GetFileNameWithoutExtension(file.FileName);
        return string.Concat("room_", filename[..(filename.Length < 12 ? filename.Length : 12)], '_', Guid.NewGuid().ToString().AsSpan(0, 20).ToString(), Path.GetExtension(file.FileName));
    }
}
