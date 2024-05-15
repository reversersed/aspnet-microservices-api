using IdentityApi.src.Data.Entities;
using IdentityApi.src.Queries;
using IdentityApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Handlers;

public class GetAuthenticatedHandler(IHttpContextAccessor _httpContextAccessor, IUserService _userService) : IRequestHandler<GetAuthenticatedUser, ActionResult<BaseResponse<UserResponse>>>
{
    private readonly IHttpContextAccessor httpContextAccessor = _httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    private readonly IUserService userService = _userService ?? throw new ArgumentNullException(nameof(userService));

    public async Task<ActionResult<BaseResponse<UserResponse>>> Handle(GetAuthenticatedUser request, CancellationToken cancellationToken)
    {
        var username = httpContextAccessor.HttpContext?.User.Identity?.Name ?? throw new CustomExceptionResponse(ResponseCodes.Unauthorized, "Имя пользователя не найдено или пользователь не авторизован");
        var user = await userService.GetUserByUsername(username) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Пользователь не существует");

        return new OkObjectResult(new BaseResponse<UserResponse> { Code = ResponseCodes.DataFound, Message = "Пользователь найден", Data = user });
    }
}
