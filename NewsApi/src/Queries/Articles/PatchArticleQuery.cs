using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Services.Entities;
using ResponsePackage;

namespace NewsApi.src.Queries.Articles;

public class PatchArticleQuery : IRequest<ActionResult<BaseResponse<ArticleDTO>>>
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
}