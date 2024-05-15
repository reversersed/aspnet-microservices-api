using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Comments;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Handlers.Comments;

public class DeleteCommentHandler(ICommentService _commentService) : IRequestHandler<DeleteCommentQuery, ActionResult<BaseResponse>>
{
    private readonly ICommentService commentService = _commentService ?? throw new ArgumentNullException(nameof(_commentService));

    public async Task<ActionResult<BaseResponse>> Handle(DeleteCommentQuery request, CancellationToken cancellationToken)
    {
        await commentService.Delete(request.Id);
        return new OkObjectResult(new BaseResponse { Code = ResponseCodes.DataDeleted, Message = "Комментарий удален" });
    }
}