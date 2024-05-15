using IdentityApi.src.Data.Entities;
using IdentityApi.src.Queries;
using IdentityApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResponsePackage;
using System.Security.Claims;

namespace IdentityApi.src.Handlers;

public class RefreshHandler(IJwtService _jwtService, SignInManager<User> _signInManager) : IRequestHandler<RefreshQuery, ActionResult<BaseResponse<AuthenticationResponse>>>
{
    private readonly IJwtService jwtService = _jwtService ?? throw new ArgumentNullException(nameof(_jwtService));
    private readonly SignInManager<User> signInManager = _signInManager ?? throw new ArgumentNullException(nameof(_signInManager));

    public async Task<ActionResult<BaseResponse<AuthenticationResponse>>> Handle(RefreshQuery request, CancellationToken cancellationToken)
    {
        ClaimsPrincipal principal;
        try
        {
            principal = jwtService.GetPrincipalFromExpiredToken(request.Token);
        }
        catch (SecurityTokenException ex)
        {
            throw new CustomExceptionResponse(ResponseCodes.BadTokenRequest, ex.Message);
        }
        var username = principal?.Identity?.Name;
        if (username is null || principal == null)
            throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Не удается получить имя пользователя из токена");

        var user = await signInManager.UserManager.FindByNameAsync(username) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Невалидный токен или пользователь не существует");
        if (user.RefreshToken != request.RefreshToken)
            throw new CustomExceptionResponse(ResponseCodes.BadTokenRequest, "Refresh token пользователя не совпадает");
        if (user.RefreshExpirationDate <= DateTime.UtcNow)
            throw new CustomExceptionResponse(ResponseCodes.BadTokenRequest, "Срок действия refresh token истек");

        string refreshToken;
        try
        {
            refreshToken = await jwtService.GenerateRefreshToken(user);
        }
        catch (ArgumentException ex)
        {
            throw new CustomExceptionResponse(ResponseCodes.BadTokenRequest, "Во время обновления произошла ошибка: " + ex.Message);
        }
        var token = await jwtService.GenerateToken(user);

        return new OkObjectResult(new BaseResponse<AuthenticationResponse> { Code = ResponseCodes.TokenRefreshed, Message = "Новый токен успешно сгенерирован", Data = new AuthenticationResponse { Token = token, RefreshToken = refreshToken } });
    }
}
