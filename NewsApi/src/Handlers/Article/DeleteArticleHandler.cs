using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Articles;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Handlers.Articles;

public class DeleteArticleHandler(IArticleService _articleService, ILogger<DeleteArticleHandler> _logger) : IRequestHandler<DeleteArticleQuery, ActionResult<BaseResponse>>
{
    private readonly ILogger<DeleteArticleHandler> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly IArticleService articleService = _articleService ?? throw new ArgumentNullException(nameof(_articleService));

    public async Task<ActionResult<BaseResponse>> Handle(DeleteArticleQuery request, CancellationToken cancellationToken)
    {
        await articleService.Delete(request.Id);
        logger.LogInformation("[NewsApi] Article with id {id} was deleted", request.Id);

        return new OkObjectResult(new BaseResponse { Code = ResponseCodes.DataDeleted, Message = "Статья удалена" });
    }
}