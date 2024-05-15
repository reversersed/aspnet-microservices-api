using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Events;

public class GetEventsPreviewQuery : IRequest<ActionResult<BaseResponse<IEnumerable<EventPreviewDTO>>>>
{
    public string Sorting { get; set; } = string.Empty;
    public long CategoryId { get; set; } = 0;
    public int Offset { get; set; } = 0;
    public int Count { get; set; } = 10;
}
