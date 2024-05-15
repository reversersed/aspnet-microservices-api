using IdentityApi.src.Data.Entities;
using IdentityApi.src.Queries;
using IdentityApi.src.RabbitMq.Interfaces;
using IdentityApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Handlers;

public class ChangeUsernameHandler(IHttpContextAccessor _httpContext, SignInManager<User> _signInManager, ILogger<ChangeUsernameHandler> _logger, IUserService _userService, IJwtService _jwtService, IRabbitService _rabbitService) : IRequestHandler<ChangeUsernameQuery, ActionResult<BaseResponse<AuthenticationResponse>>>
{
    private readonly IJwtService jwtService = _jwtService ?? throw new ArgumentNullException(nameof(_jwtService));
    private readonly IUserService userService = _userService ?? throw new ArgumentNullException(nameof(_userService));
    private readonly IHttpContextAccessor httpContext = _httpContext ?? throw new ArgumentNullException(nameof(_httpContext));
    private readonly ILogger<ChangeUsernameHandler> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly IRabbitService rabbitService = _rabbitService ?? throw new ArgumentNullException(nameof(_rabbitService));
    private readonly SignInManager<User> signInManager = _signInManager ?? throw new ArgumentNullException(nameof(_signInManager));

    public async Task<ActionResult<BaseResponse<AuthenticationResponse>>> Handle(ChangeUsernameQuery request, CancellationToken cancellationToken)
    {
        var current_username = httpContext.HttpContext?.User.Identity?.Name ?? throw new CustomExceptionResponse(ResponseCodes.Unauthorized, "Пользователь не авторизован");

        var usr = await signInManager.UserManager.FindByNameAsync(current_username) ?? throw new CustomExceptionResponse(ResponseCodes.Unauthorized, "Пользователь не найден");
        if ((await signInManager.UserManager.CheckPasswordAsync(usr, request.Password)) == false)
            throw new CustomExceptionResponse(ResponseCodes.Unauthorized, "Указан неверный пароль");

        var user = await userService.UpdateUsername(current_username, request.UserName);

        logger.LogInformation("User {oldname} id = {id} changed username to {newname}", current_username, user.Id, request.UserName);
        rabbitService.SendUsernameChangedEvent(current_username, request.UserName);
        return new OkObjectResult(new BaseResponse<AuthenticationResponse>
        {
            Code = ResponseCodes.DataUpdated,
            Message = "Имя пользователя успешно изменено",
            Data = new AuthenticationResponse
            {
                Token = await jwtService.GenerateToken(user),
                RefreshToken = await jwtService.GenerateRefreshToken(user)
            }
        });
    }
}
