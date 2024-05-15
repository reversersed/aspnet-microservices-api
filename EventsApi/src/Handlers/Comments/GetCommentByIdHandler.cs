using EventsApi.src.Queries.Comments;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Comments;

public class GetCommentByIdHandler(ICommentService _commentService) : IRequestHandler<GetCommentByIdQuery, ActionResult<BaseResponse<CommentDTO>>>
{
    private readonly ICommentService commentService = _commentService ?? throw new ArgumentNullException(nameof(_commentService));
    public async Task<ActionResult<BaseResponse<CommentDTO>>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await commentService.Get(request.Id);
        return new OkObjectResult(new BaseResponse<CommentDTO> { Code = ResponseCodes.DataFound, Message = "Комментарий найден", Data = response });
    }
}
