using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace NewsApi.src.Queries.Comments;

public class DeleteCommentQuery : IRequest<ActionResult<BaseResponse>>
{
    public long Id { get; set; } = 0;
}