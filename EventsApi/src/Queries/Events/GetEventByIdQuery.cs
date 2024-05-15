using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Events;

public class GetEventByIdQuery : IRequest<ActionResult<BaseResponse<EventDTO>>>
{
    public long Id { get; set; } = 0;
}
