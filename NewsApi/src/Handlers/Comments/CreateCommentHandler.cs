using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsApi.src.Queries.Comments;
using NewsApi.src.Services.Entities;
using NewsApi.src.Services.Interfaces;
using ResponsePackage;

namespace NewsApi.src.Handlers.Comments;

public class CreateCommentHandler(ICommentService _commentService, IHttpContextAccessor _httpContext) : IRequestHandler<CreateCommentQuery, ActionResult<BaseResponse<CommentDTO>>>
{
    private readonly ICommentService commentService = _commentService ?? throw new ArgumentNullException(nameof(_commentService));
    private readonly IHttpContextAccessor httpContext = _httpContext ?? throw new ArgumentNullException(nameof(_httpContext));

    public async Task<ActionResult<BaseResponse<CommentDTO>>> Handle(CreateCommentQuery request, CancellationToken cancellationToken)
    {
        var username = httpContext.HttpContext?.User.Identity?.Name ?? throw new CustomExceptionResponse(ResponseCodes.Unauthorized, "Не удается получить имя пользователя или пользователь не авторизован");
        var response = await commentService.Add(request.ArticleId, new CommentDTO { Author = username, Content = request.Content });
        return new CreatedAtActionResult(null, null, null, new BaseResponse<CommentDTO> { Code = ResponseCodes.DataCreated, Message = "Комментарий добавлен", Data = response });
    }
}