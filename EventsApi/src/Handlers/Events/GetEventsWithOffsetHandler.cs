using EventsApi.src.Queries.Events;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Events;

public class GetEventsWithOffsetHandler(IEventService _eventService) : IRequestHandler<GetEventsWithOffsetQuery, ActionResult<BaseResponse<IEnumerable<EventDTO>>>>
{
    private readonly IEventService eventService = _eventService ?? throw new ArgumentNullException(nameof(_eventService));
    public async Task<ActionResult<BaseResponse<IEnumerable<EventDTO>>>> Handle(GetEventsWithOffsetQuery request, CancellationToken cancellationToken)
    {
        var data = await eventService.Get(request.CategoryId, request.Sorting, request.Offset, request.Count);
        return new OkObjectResult(new BaseResponse<IEnumerable<EventDTO>> { Code = ResponseCodes.DataFound, Message = $"Найдено {data.Count()} мероприятий", Data = data });
    }
}