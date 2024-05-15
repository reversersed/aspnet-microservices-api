using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Services.Entities;
using ResponsePackage;

namespace NewsApi.src.Queries.Articles;

public class FindArticleByIdQuery : IRequest<ActionResult<BaseResponse<ArticleDTO>>>
{
    public long Id { get; set; } = 0;
}