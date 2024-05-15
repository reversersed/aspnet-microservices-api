using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Services.Entities;
using ResponsePackage;

namespace NewsApi.src.Queries.Articles;

public class CreateArticleQuery : IRequest<ActionResult<BaseResponse<ArticleDTO>>>
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
}