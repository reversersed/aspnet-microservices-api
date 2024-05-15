using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Services.Entities;
using ResponsePackage;

namespace NewsApi.src.Queries.Comments;

public class PatchCommentQuery : IRequest<ActionResult<BaseResponse<CommentDTO>>>
{
    public long Id { get; set; } = 0;
    public string? Content { get; set; }
}