using EventsApi.src.Queries.Comments;
using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Controllers;
[Route("[controller]")]
[ApiController]
public class CommentsController(IMediator _mediator) : ControllerBase
{
    private readonly IMediator mediator = _mediator ?? throw new ArgumentNullException(nameof(_mediator));

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<CommentDTO>>> Get(long id)
    {
        return await mediator.Send(new GetCommentByIdQuery { Id = id });
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse>> Delete(long id)
    {
        return await mediator.Send(new DeleteCommentQuery { Id = id });
    }
    [HttpPatch("{id}")]
    public async Task<ActionResult<BaseResponse<CommentDTO>>> Patch(long id, [FromBody] UpdateCommentQuery query)
    {
        query.Id = id;
        return await mediator.Send(query);
    }
}