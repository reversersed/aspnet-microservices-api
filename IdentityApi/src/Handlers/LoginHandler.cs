using IdentityApi.src.Data.Entities;
using IdentityApi.src.Queries;
using IdentityApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Handlers;

public class LoginHandler(SignInManager<User> _signInManager, IJwtService _jwtService) : IRequestHandler<LoginQuery, ActionResult<BaseResponse<AuthenticationResponse>>>
{
    private readonly SignInManager<User> signInManager = _signInManager ?? throw new ArgumentNullException(nameof(_signInManager));
    private readonly IJwtService jwtService = _jwtService ?? throw new ArgumentNullException(nameof(_jwtService));

    public async Task<ActionResult<BaseResponse<AuthenticationResponse>>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await signInManager.UserManager.FindByNameAsync(request.Username) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Пользователь не найден");

        var passwordValidation = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if(passwordValidation.Succeeded)
        {
            return new OkObjectResult(new BaseResponse<AuthenticationResponse>
            {
                Code = ResponseCodes.LoginSuccess,
                Message = "Пользователь авторизован",
                Data = new AuthenticationResponse
                {
                    Token = await jwtService.GenerateToken(user, request.RememberMe),
                    RefreshToken = await jwtService.GenerateRefreshToken(user, request.RememberMe)
                }
            });
        }
        throw new CustomExceptionResponse(ResponseCodes.BadLoginRequest, "Неверный пароль");
    }
}
