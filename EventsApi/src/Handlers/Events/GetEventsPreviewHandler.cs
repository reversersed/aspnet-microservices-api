using EventsApi.src.Queries.Events;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Events;

public class GetEventsPreviewHandler(IEventService _eventService) : IRequestHandler<GetEventsPreviewQuery, ActionResult<BaseResponse<IEnumerable<EventPreviewDTO>>>>
{
    private readonly IEventService eventService = _eventService ?? throw new ArgumentNullException(nameof(_eventService));

    public async Task<ActionResult<BaseResponse<IEnumerable<EventPreviewDTO>>>> Handle(GetEventsPreviewQuery request, CancellationToken cancellationToken)
    {
        var data = await eventService.GetPreview(request.CategoryId, request.Sorting, request.Offset, request.Count);
        return new OkObjectResult(new BaseResponse<IEnumerable<EventPreviewDTO>> { Code = ResponseCodes.DataFound, Message = $"Найдено {data.Count()} мероприятий", Data = data });
    }
}
