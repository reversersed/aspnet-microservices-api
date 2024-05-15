using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Articles;
using NewsApi.src.Services.Entities;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Handlers.Articles;

public class GetArticlesWithOffsetHandler(IArticleService _articleService, ILogger<GetArticlesWithOffsetHandler> _logger) : IRequestHandler<GetArticlesWithOffsetQuery, ActionResult<BaseResponse<IEnumerable<ArticleDTO>>>>
{
    private readonly ILogger<GetArticlesWithOffsetHandler> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly IArticleService articleService = _articleService ?? throw new ArgumentNullException(nameof(_articleService));

    public async Task<ActionResult<BaseResponse<IEnumerable<ArticleDTO>>>> Handle(GetArticlesWithOffsetQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("[NewsApi] Requested {count} articles with offset {offset}, proceeding...", request.Count, request.Offset);

        var result = await articleService.Get(request.Offset, request.Count);
        return new OkObjectResult(new BaseResponse<IEnumerable<ArticleDTO>> { Code = ResponseCodes.DataFound, Message = $"Найдено {result.Count()} статей", Data = result });
    }
}