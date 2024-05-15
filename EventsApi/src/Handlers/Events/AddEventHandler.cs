using EventsApi.src.Data.Entities;
using System.Runtime.InteropServices;
using System;
using EventsApi.src.Queries.Events;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Events;

public class AddEventHandler(IEventService _eventService, IWebHostEnvironment _environment, IHttpContextAccessor _httpContextAccessor) : IRequestHandler<AddEventQuery, ActionResult<BaseResponse<EventDTO>>>
{
    private readonly IEventService eventServicve = _eventService ?? throw new ArgumentNullException(nameof(_eventService));
    private readonly IHttpContextAccessor httpContextAccessor = _httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));
    private readonly IWebHostEnvironment environment = _environment ?? throw new ArgumentNullException(nameof(environment));

    public async Task<ActionResult<BaseResponse<EventDTO>>> Handle(AddEventQuery request, CancellationToken cancellationToken)
    {
        var username = httpContextAccessor.HttpContext?.User.Identity?.Name ?? throw new CustomExceptionResponse(ResponseCodes.Unauthorized, "Не удается получить имя пользователя");

        var event_dto = request.Adapt<EventDTO>();
        event_dto.Creator = username;

#pragma warning disable CS8604 // Валидируется в пайплайне
        var filePath = Path.Combine(environment.ContentRootPath, "files", CreateUniqueFileName(request.Image));
#pragma warning restore CS8604
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "");
        else
            Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "", UnixFileMode.OtherRead | UnixFileMode.UserWrite);

        event_dto.EventImage = filePath;
        var entity = await eventServicve.Add(event_dto, request.CategoryId);

        await request.Image.CopyToAsync(new FileStream(filePath, FileMode.Create), cancellationToken); // Writing after successfull adding
        return new CreatedAtActionResult(null, null, null, new BaseResponse<EventDTO> { Code = ResponseCodes.DataCreated, Message = "Мероприятие успешно создано", Data = entity });
    }
    private static string CreateUniqueFileName(IFormFile file)
    {
        var filename = Path.GetFileNameWithoutExtension(file.FileName);
        return string.Concat("event_", filename[..(filename.Length < 12 ? filename.Length : 12)], '_', Guid.NewGuid().ToString().AsSpan(0, 20).ToString(), Path.GetExtension(file.FileName));
    }
}
