using EventsApi.src.Queries.Events;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Events;

public class GetEventByIdHandler(IEventService _eventService) : IRequestHandler<GetEventByIdQuery, ActionResult<BaseResponse<EventDTO>>>
{
    private readonly IEventService eventService = _eventService ?? throw new ArgumentNullException(nameof(eventService));

    public async Task<ActionResult<BaseResponse<EventDTO>>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var obj = await eventService.Get(request.Id);
        return new OkObjectResult(new BaseResponse<EventDTO> { Code = ResponseCodes.DataFound, Message = "Мероприятие найдено", Data = obj });
    }
}
