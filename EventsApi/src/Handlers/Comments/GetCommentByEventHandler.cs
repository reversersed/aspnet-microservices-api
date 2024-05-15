using EventsApi.src.Queries.Comments;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Comments;

public class GetCommentByEventHandler(ICommentService _commentService) : IRequestHandler<GetCommentByEventQuery, ActionResult<BaseResponse<IEnumerable<CommentDTO>>>>
{
    private readonly ICommentService commentService = _commentService ?? throw new ArgumentNullException(nameof(_commentService));

    public async Task<ActionResult<BaseResponse<IEnumerable<CommentDTO>>>> Handle(GetCommentByEventQuery request, CancellationToken cancellationToken)
    {
        var response = await commentService.GetByEvent(request.EventId, request.Offset, request.Count);
        return new OkObjectResult(new BaseResponse<IEnumerable<CommentDTO>> { Code = ResponseCodes.DataFound, Message = $"Найдено {response.Count()} комментариев", Data = response });
    }
}
