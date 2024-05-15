using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Comments;

public class GetCommentByEventQuery : IRequest<ActionResult<BaseResponse<IEnumerable<CommentDTO>>>>
{
    public long EventId { get; set; } = 0;
    public int Offset { get; set; } = 0;
    public int Count = 15;
}
