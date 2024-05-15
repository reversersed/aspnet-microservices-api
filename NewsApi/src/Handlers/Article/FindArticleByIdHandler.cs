using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Articles;
using NewsApi.src.Services.Entities;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Handlers.Articles;

public class FindArticleByIdHandler(IArticleService _articleService) : IRequestHandler<FindArticleByIdQuery, ActionResult<BaseResponse<ArticleDTO>>>
{
    private readonly IArticleService articleService = _articleService ?? throw new ArgumentNullException(nameof(_articleService));

    public async Task<ActionResult<BaseResponse<ArticleDTO>>> Handle(FindArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await articleService.Get(request.Id);
        return new OkObjectResult(new BaseResponse<ArticleDTO> { Code = ResponseCodes.DataFound, Message = "Статья успеешно найдена", Data = result });
    }
}