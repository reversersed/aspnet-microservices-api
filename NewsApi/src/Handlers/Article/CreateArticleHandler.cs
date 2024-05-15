using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Articles;
using NewsApi.src.Services.Entities;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Handlers.Articles;

public class CreateArticleHandler(IArticleService _articleService, ILogger<CreateArticleHandler> _logger, IHttpContextAccessor _httpContext) : IRequestHandler<CreateArticleQuery, ActionResult<BaseResponse<ArticleDTO>>>
{
    private readonly ILogger<CreateArticleHandler> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly IArticleService articleService = _articleService ?? throw new ArgumentNullException(nameof(_articleService));
    private readonly IHttpContextAccessor httpContext = _httpContext ?? throw new ArgumentNullException(nameof(_httpContext));

    public async Task<ActionResult<BaseResponse<ArticleDTO>>> Handle(CreateArticleQuery request, CancellationToken cancellationToken)
    {
        var username = httpContext.HttpContext?.User.Identity?.Name ?? throw new CustomExceptionResponse(ResponseCodes.Unauthorized, "Не удается получить имя пользователя или пользователь не авторизован");
        var article = new ArticleDTO { Author = username, Content = request.Content, Title = request.Title };
        var response = await articleService.Add(article);

        logger.LogInformation("[NewsApi] Article {title} was created with Id {id} by {username}", request.Title, response.Id, username);
        return new CreatedAtActionResult(null, null, new { id = response.Id }, new BaseResponse<ArticleDTO> { Code = ResponseCodes.DataCreated, Message = "Статья добавлена", Data = response });
    }
}