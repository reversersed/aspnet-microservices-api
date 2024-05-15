using EventsApi.src.Queries.Comments;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Comments;

public class AddCommentHandler(ICommentService _commentService, IHttpContextAccessor _httpContextAccessor) : IRequestHandler<AddCommentQuery, ActionResult<BaseResponse<CommentDTO>>>
{
    private readonly ICommentService commentService = _commentService ?? throw new ArgumentNullException(nameof(_commentService));
    private readonly IHttpContextAccessor httpContextAccessor = _httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));

    public async Task<ActionResult<BaseResponse<CommentDTO>>> Handle(AddCommentQuery request, CancellationToken cancellationToken)
    {
        var username = httpContextAccessor.HttpContext?.User.Identity?.Name ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Не удается получить имя пользователя или пользователь не авторизован");
        var comment = new CommentDTO { 
            Content = request.Content,
            Creator = username
        };
        var response = await commentService.Add(comment, request.EventId);
        return new CreatedAtActionResult(null, null, null, new BaseResponse<CommentDTO> { Code = ResponseCodes.DataCreated, Message = "Комментарий добавлен", Data = response });
    }
}
