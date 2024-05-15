using EventsApi.src.Queries.Comments;
using EventsApi.src.Queries.Events;
using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Controllers;

[Route("[controller]")]
[ApiController]
public class EventsController(IMediator _mediator) : ControllerBase
{
    private readonly IMediator mediator = _mediator ?? throw new ArgumentNullException(nameof(_mediator));

    [HttpGet("preview")]
    public async Task<ActionResult<BaseResponse<IEnumerable<EventPreviewDTO>>>> GetPreviews([FromQuery] int offset = 0, [FromQuery] int count = 10, [FromQuery] long category = 0, [FromQuery] string sorting = "newest")
    {
        return await mediator.Send(new GetEventsPreviewQuery { CategoryId = category, Count = count, Offset = offset, Sorting = sorting });
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<EventDTO>>> Get(long id)
    {
        return await mediator.Send(new GetEventByIdQuery { Id = id });
    }
    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<EventDTO>>>> Get([FromQuery] int offset = 0, [FromQuery] int count = 10, [FromQuery] long category = 0, [FromQuery] string sorting = "newest")
    {
        return await mediator.Send(new GetEventsWithOffsetQuery { CategoryId = category, Count = count, Offset = offset, Sorting = sorting });
    }
    [HttpPost]
    public async Task<ActionResult<BaseResponse<EventDTO>>> Add([FromForm] AddEventQuery query)
    {
        return await mediator.Send(query);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse>> Delete(long id)
    {
        return await mediator.Send(new DeleteEventQuery { Id = id });
    }
    [HttpPatch("{id}")]
    public async Task<ActionResult<BaseResponse<EventDTO>>> Update(long id, [FromForm] UpdateEventQuery query)
    {
        query.Id = id;
        return await mediator.Send(query);
    }
    [HttpGet("{id}/comments")]
    public async Task<ActionResult<BaseResponse<IEnumerable<CommentDTO>>>> GetComments(long id, [FromQuery] int offset = 0, [FromQuery] int count = 10)
    {
        return await mediator.Send(new GetCommentByEventQuery { EventId = id, Offset = offset, Count = count});
    }
    [HttpPost("{id}/comments")]
    public async Task<ActionResult<BaseResponse<CommentDTO>>> AddComment(long id, [FromBody] AddCommentQuery query)
    {
        query.EventId = id;
        return await mediator.Send(query);
    }
}