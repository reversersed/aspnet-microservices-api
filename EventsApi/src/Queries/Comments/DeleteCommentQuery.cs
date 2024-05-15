using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Comments;

public class DeleteCommentQuery : IRequest<ActionResult<BaseResponse>>
{
    public long Id { get; set; }
}
