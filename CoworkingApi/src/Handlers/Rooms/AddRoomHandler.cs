using System.Runtime.InteropServices;
using System;
using CoworkingApi.src.Queries.Rooms;
using CoworkingApi.src.Services.Entities;
using CoworkingApi.src.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Handlers.Rooms;

public class AddRoomHandler(IRoomService roomService, IWebHostEnvironment _environment) : IRequestHandler<AddRoomQuery, ActionResult<BaseResponse<RoomDTO>>>
{
    private readonly IRoomService _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
    private readonly IWebHostEnvironment environment = _environment ?? throw new ArgumentNullException(nameof(_environment));
    public async Task<ActionResult<BaseResponse<RoomDTO>>> Handle(AddRoomQuery request, CancellationToken cancellationToken)
    {
        var room_dto = request.Adapt<RoomDTO>();

#pragma warning disable CS8604 // Валидируется в пайплайне
        var filePath = Path.Combine(environment.ContentRootPath, "files", CreateUniqueFileName(request.RoomImage));

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "");
        else
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "", UnixFileMode.OtherRead | UnixFileMode.UserWrite);
#pragma warning restore CS8604

        room_dto.RoomImage = filePath;
        var entity = await _roomService.Add(room_dto);

        await request.RoomImage.CopyToAsync(new FileStream(filePath, FileMode.Create), cancellationToken);
        return new CreatedAtActionResult(null, null, null, new BaseResponse<RoomDTO> { Code = ResponseCodes.DataCreated, Message = request.RoomImage.FileName, Data = entity });
    }
    private static string CreateUniqueFileName(IFormFile file)
    {
        var filename = Path.GetFileNameWithoutExtension(file.FileName);
        return string.Concat("room_", filename[..(filename.Length < 12 ? filename.Length : 12)], '_', Guid.NewGuid().ToString().AsSpan(0, 20).ToString(), Path.GetExtension(file.FileName));
    }
}
