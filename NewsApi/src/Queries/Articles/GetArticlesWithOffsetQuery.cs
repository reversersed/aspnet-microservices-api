using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Services.Entities;
using ResponsePackage;

namespace NewsApi.src.Queries.Articles;

public class GetArticlesWithOffsetQuery : IRequest<ActionResult<BaseResponse<IEnumerable<ArticleDTO>>>>
{
    public int Offset { get; set; } = 0;
    public int Count { get; set; } = 10;
}