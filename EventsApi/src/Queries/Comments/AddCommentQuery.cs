using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Comments;

public class AddCommentQuery : IRequest<ActionResult<BaseResponse<CommentDTO>>>
{
    public long EventId { get; set; } = 0;
    public string Content { get; set; } = string.Empty;
}
