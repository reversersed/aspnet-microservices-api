using EventsApi.src.Data.Entities;
using System.Runtime.InteropServices;
using System;
using EventsApi.src.Queries.Events;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ResponsePackage;
using Microsoft.Extensions.Hosting;

namespace EventsApi.src.Handlers.Events;

public class UpdateEventHandler(IEventService _eventService, IWebHostEnvironment _environment) : IRequestHandler<UpdateEventQuery, ActionResult<BaseResponse<EventDTO>>>
{
    private readonly IEventService eventService = _eventService ?? throw new ArgumentNullException(nameof(eventService));
    private readonly IWebHostEnvironment environment = _environment ?? throw new ArgumentNullException(nameof(environment));

    public async Task<ActionResult<BaseResponse<EventDTO>>> Handle(UpdateEventQuery request, CancellationToken cancellationToken)
    {
        var dto = await eventService.Get(request.Id);
        var cfg = TypeAdapterConfig.GlobalSettings.Clone();
        cfg.Default.IgnoreNullValues(true);
        dto = request.Adapt(dto, cfg);

        if(request.Image != null)
        {
            if (dto.EventImage.Length > 0 && File.Exists(dto.EventImage))
                File.Delete(dto.EventImage);

            var filePath = Path.Combine(environment.ContentRootPath, "files", CreateUniqueFileName(request.Image));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "");
            else
                Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? "", UnixFileMode.OtherRead | UnixFileMode.UserWrite);

            dto.EventImage = filePath;
            await request.Image.CopyToAsync(new FileStream(filePath, FileMode.Create), cancellationToken); // writing after successfull updating
        }

        var result = await eventService.Update(dto, request.CategoryId ?? dto.Category.Id);
        return new OkObjectResult(new BaseResponse<EventDTO> { Code = ResponseCodes.DataUpdated, Message = "Мероприятие изменено", Data = result });
    }
    private static string CreateUniqueFileName(IFormFile file)
    {
        var filename = Path.GetFileNameWithoutExtension(file.FileName);
        return string.Concat("event_", filename[..(filename.Length < 12 ? filename.Length : 12)], '_', Guid.NewGuid().ToString().AsSpan(0, 20).ToString(), Path.GetExtension(file.FileName));
    }
}
