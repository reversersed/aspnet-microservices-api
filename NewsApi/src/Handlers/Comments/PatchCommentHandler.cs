using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Comments;
using NewsApi.src.Services.Entities;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Handlers.Comments;

public class PatchCommentHandler(ICommentService _commentService) : IRequestHandler<PatchCommentQuery, ActionResult<BaseResponse<CommentDTO>>>
{
    private readonly ICommentService commentService = _commentService ?? throw new ArgumentNullException(nameof(_commentService));

    public async Task<ActionResult<BaseResponse<CommentDTO>>> Handle(PatchCommentQuery request, CancellationToken cancellationToken)
    {
        var response = await commentService.Patch(request.Id, request.Content);
        return new OkObjectResult(new BaseResponse<CommentDTO> { Code = ResponseCodes.DataUpdated, Message = "Комментарий успешно обновлен", Data = response });
    }
}