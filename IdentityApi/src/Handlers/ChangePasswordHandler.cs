using Extensions;
using IdentityApi.src.Data.Entities;
using IdentityApi.src.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Handlers;

public class ChangePasswordHandler(SignInManager<User> _signInManager, IHttpContextAccessor _httpContextAccessor) : IRequestHandler<ChangePasswordQuery, ActionResult<BaseResponse>>
{
    private readonly SignInManager<User> signInManager = _signInManager ?? throw new ArgumentNullException(nameof(_signInManager));
    private readonly IHttpContextAccessor httpContextAccessor = _httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));

    public async Task<ActionResult<BaseResponse>> Handle(ChangePasswordQuery request, CancellationToken cancellationToken)
    {
        var username = httpContextAccessor.HttpContext?.User.Identity?.Name ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Не удалось получить имя пользователя");
        var user = await signInManager.UserManager.FindByNameAsync(username) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Не удалось найти пользователя");
        if (!(await signInManager.UserManager.CheckPasswordAsync(user, request.CurrentPassword)))
            throw new CustomExceptionResponse(ResponseCodes.ValidationError, "Указан неверный пароль");

        var result = await signInManager.UserManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(i => i.Description).ToList();
            throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, "Не удалось сменить пароль", errors);
        }
        if (user.Email is null || user.UserName is null)
            throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, "Невалидный пользователь");
        await EmailService.SendEmailAsync(user.Email, "Пароль сменен", EmailService.CreatePasswordRecoveryNotificationMessage(user.UserName));
        return new BaseResponse { Code = ResponseCodes.DataUpdated, Message = "Пароль успешно изменен" };
    }
}
