using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace NewsApi.src.Queries.Articles;

public class DeleteArticleQuery : IRequest<ActionResult<BaseResponse>>
{
    public long Id { get; set; } = 0;
}