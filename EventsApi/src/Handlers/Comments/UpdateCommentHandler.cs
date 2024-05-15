using EventsApi.src.Queries.Comments;
using EventsApi.src.Repositories.Interfaces;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Comments;

public class UpdateCommentHandler(ICommentService _commentService) : IRequestHandler<UpdateCommentQuery, ActionResult<BaseResponse<CommentDTO>>>
{
    private readonly ICommentService commentService = _commentService ?? throw new ArgumentNullException(nameof(_commentService));
    public async Task<ActionResult<BaseResponse<CommentDTO>>> Handle(UpdateCommentQuery request, CancellationToken cancellationToken)
    {
        var comment = await commentService.Get(request.Id);
        _ = request.Adapt(comment);

        var response = await commentService.Update(comment);
        return new OkObjectResult(new BaseResponse<CommentDTO> { Code = ResponseCodes.DataUpdated, Message = "Комментарий изменен", Data = response });
    }
}
