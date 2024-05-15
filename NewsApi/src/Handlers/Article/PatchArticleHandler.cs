using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Articles;
using NewsApi.src.Services.Entities;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Handlers.Articles;

public class PatchArticleHandler(IArticleService _articleService) : IRequestHandler<PatchArticleQuery, ActionResult<BaseResponse<ArticleDTO>>>
{
    private readonly IArticleService articleService = _articleService ?? throw new ArgumentNullException(nameof(_articleService));

    public async Task<ActionResult<BaseResponse<ArticleDTO>>> Handle(PatchArticleQuery request, CancellationToken cancellationToken)
    {
        var result = await articleService.Patch(request.Id, request.Title, request.Content);

        return new OkObjectResult(new BaseResponse<ArticleDTO> { Code = ResponseCodes.DataUpdated, Message = "Статья успешно обновлена", Data = result });
    }
}