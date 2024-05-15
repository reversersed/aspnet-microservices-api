using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Comments;

public class UpdateCommentQuery : IRequest<ActionResult<BaseResponse<CommentDTO>>>
{
    public long Id { get; set; }
    public string Content { get; set; }
}
