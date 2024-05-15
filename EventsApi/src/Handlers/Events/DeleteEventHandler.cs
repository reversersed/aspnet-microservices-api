using EventsApi.src.Queries.Events;
using EventsApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Events;

public class DeleteEventHandler(IEventService _eventService) : IRequestHandler<DeleteEventQuery, ActionResult<BaseResponse>>
{
    private readonly IEventService eventService = _eventService ?? throw new ArgumentNullException(nameof(_eventService));

    public async Task<ActionResult<BaseResponse>> Handle(DeleteEventQuery request, CancellationToken cancellationToken)
    {
        await eventService.Delete(request.Id);
        return new OkObjectResult(new BaseResponse { Code = ResponseCodes.DataDeleted, Message = "Мероприятие успешно удалено" });
    }
}
