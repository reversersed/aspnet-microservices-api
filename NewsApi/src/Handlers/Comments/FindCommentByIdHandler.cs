using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Comments;
using NewsApi.src.Services.Entities;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Handlers.Comments;

public class FindCommentByIdHandler(ICommentService _commentService) : IRequestHandler<FindCommentByIdQuery, ActionResult<BaseResponse<CommentDTO>>>
{
    private readonly ICommentService commentService = _commentService ?? throw new ArgumentNullException(nameof(_commentService));

    public async Task<ActionResult<BaseResponse<CommentDTO>>> Handle(FindCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await commentService.Get(request.Id);
        return new OkObjectResult(new BaseResponse<CommentDTO> { Code = ResponseCodes.DataFound, Message = "Комментарий получен", Data = response });
    }
}