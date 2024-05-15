using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Comments;
using NewsApi.src.Services.Entities;
using ResponsePackage;

namespace NewsApi.src.Controllers;

[Route("[controller]")]
[ApiController]
public class CommentsController(IMediator _mediator) : ControllerBase
{
    private readonly IMediator mediator = _mediator ?? throw new ArgumentNullException(nameof(_mediator));

    [HttpPatch("{id}")]
    public async Task<ActionResult<BaseResponse<CommentDTO>>> PatchComment(long id, [FromBody] PatchCommentQuery query)
    {
        query.Id = id;
        return await mediator.Send(query);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse>> DeleteComment(long id)
    {
        return await mediator.Send(new DeleteCommentQuery { Id = id });
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<CommentDTO>>> GetComment(long id)
    {
        return await mediator.Send(new FindCommentByIdQuery { Id = id });
    }
}