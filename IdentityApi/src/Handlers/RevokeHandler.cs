using IdentityApi.src.Data.Entities;
using IdentityApi.src.Queries;
using IdentityApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Handlers;

public class RevokeHandler(IJwtService _jwtService, SignInManager<User> _signInManager, IHttpContextAccessor _httpContextAccessor) : IRequestHandler<RevokeQuery, ActionResult<BaseResponse>>
{
    private readonly IJwtService jwtService = _jwtService ?? throw new ArgumentNullException(nameof(_jwtService));
    private readonly SignInManager<User> signInManager = _signInManager ?? throw new ArgumentNullException(nameof(_signInManager));
    private readonly IHttpContextAccessor httpContextAccessor = _httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));

    public async Task<ActionResult<BaseResponse>> Handle(RevokeQuery request, CancellationToken cancellationToken)
    {
        var username = (httpContextAccessor.HttpContext?.User.Identity?.Name) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Пользователь не авторизован");
        var user = await signInManager.UserManager.FindByNameAsync(username) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound,"Пользователь не найден");
        await jwtService.RevokeToken(user);
        return new OkObjectResult(new BaseResponse { Code = ResponseCodes.TokenRevoked, Message = "Токен пользователя был сброшен" });
    }
}
