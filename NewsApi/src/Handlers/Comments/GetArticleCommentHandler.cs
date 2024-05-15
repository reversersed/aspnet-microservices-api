using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Comments;
using NewsApi.src.Services.Entities;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Handlers.Comments;

public class GetArticleCommentHandler(ICommentService _commentService) : IRequestHandler<GetArticleCommentQuery, ActionResult<BaseResponse<IEnumerable<CommentDTO>>>>
{
    private readonly ICommentService commentService = _commentService ?? throw new ArgumentNullException(nameof(_commentService));

    public async Task<ActionResult<BaseResponse<IEnumerable<CommentDTO>>>> Handle(GetArticleCommentQuery request, CancellationToken cancellationToken)
    {
        var response = await commentService.Get(request.ArticleId, request.Offset, request.Count);
        return new OkObjectResult(new BaseResponse<IEnumerable<CommentDTO>> { Code = ResponseCodes.DataFound, Message = $"Найдено {response.Count()} комментариев", Data = response });
    }
}