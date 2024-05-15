using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Services.Entities;
using ResponsePackage;

namespace NewsApi.src.Queries.Comments;

public class GetArticleCommentQuery : IRequest<ActionResult<BaseResponse<IEnumerable<CommentDTO>>>>
{
    public long ArticleId { get; set; } = 0;
    public int Offset { get; set; } = 0;
    public int Count { get; set; } = 10;
}
