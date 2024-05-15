using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Articles;
using NewsApi.src.Queries.Comments;
using NewsApi.src.Services.Entities;
using ResponsePackage;
using System.Formats.Asn1;

namespace NewsApi.src.Controllers;

[Route("[controller]")]
[ApiController]
public class ArticlesController(IMediator _mediator) : ControllerBase
{
    private readonly IMediator mediator = _mediator ?? throw new ArgumentNullException(nameof(_mediator));

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<ArticleDTO>>> GetById(long id)
    {
        return await mediator.Send(new FindArticleByIdQuery { Id = id });
    }
    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<ArticleDTO>>>> Get([FromQuery] int offset = 0, [FromQuery] int count = 10)
    {
        return await mediator.Send(new GetArticlesWithOffsetQuery { Offset = offset, Count = count });
    }
    [HttpPost]
    public async Task<ActionResult<BaseResponse<ArticleDTO>>> InsertArticle([FromBody] CreateArticleQuery query)
    {
        return await mediator.Send(query);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse>> DeleteArticle(long id)
    {
        return await mediator.Send(new DeleteArticleQuery { Id = id });
    }
    [HttpPatch("{id}")]
    public async Task<ActionResult<BaseResponse<ArticleDTO>>> PatchArticle(long id, [FromBody] PatchArticleQuery query)
    {
        query.Id = id;
        return await mediator.Send(query);
    }
    [HttpPost("{id}/comments")]
    public async Task<ActionResult<BaseResponse<CommentDTO>>> CreateComment(long id, [FromBody] CreateCommentQuery query)
    {
        query.ArticleId = id;
        return await mediator.Send(query);
    }
    [HttpGet("{id}/comments")]
    public async Task<ActionResult<BaseResponse<IEnumerable<CommentDTO>>>> GetComments(long id, [FromQuery] int offset = 0, [FromQuery] int count = 10)
    {
        return await mediator.Send(new GetArticleCommentQuery { ArticleId = id, Offset = offset, Count = count});
    }
}