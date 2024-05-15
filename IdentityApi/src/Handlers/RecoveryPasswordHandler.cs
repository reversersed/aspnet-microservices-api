using Extensions;
using IdentityApi.src.Data.Entities;
using IdentityApi.src.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ResponsePackage;

namespace IdentityApi.src.Handlers;

public class RecoveryPasswordHandler(SignInManager<User> _signInManager, IMemoryCache _cache) : IRequestHandler<RecoveryPasswordQuery, ActionResult<BaseResponse>>
{
    private readonly SignInManager<User> signInManager = _signInManager ?? throw new ArgumentNullException(nameof(_signInManager));
    private readonly IMemoryCache cache = _cache ?? throw new ArgumentNullException(nameof(_cache));

    public async Task<ActionResult<BaseResponse>> Handle(RecoveryPasswordQuery request, CancellationToken cancellationToken)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(request.Email) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Пользователя с такой почтой не существует");
        if(user.UserName is null)
            throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, "Невалидный пользователь");
        if (!cache.TryGetValue(request.Email + "_code", out int email_code))
        {
            var code = EmailService.GenerateNewCode();
            cache.Set(request.Email + "_code", code, TimeSpan.FromMinutes(15));
            cache.Set(request.Email + "_timeout", DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 30, TimeSpan.FromSeconds(30));
            await EmailService.SendEmailAsync(request.Email, "Подтверждение почты", EmailService.CreatePasswordRecoveryEmailMessage(user.UserName, code));
            return new UnauthorizedObjectResult(new BaseResponse
            {
                Code = ResponseCodes.Unauthorized,
                Message = "Требуется подтверждение почты"
            });
        }
        else if (request.Code == 0)
        {
            if (cache.TryGetValue(request.Email + "_timeout", out long timeLeft))
                return new UnauthorizedObjectResult(new BaseResponse
                {
                    Code = ResponseCodes.Unauthorized,
                    Message = $"До повторной отправки осталось {timeLeft - DateTimeOffset.UtcNow.ToUnixTimeSeconds()} секунд(ы)"
                });
            var code = EmailService.GenerateNewCode();
            cache.Set(request.Email + "_code", code, TimeSpan.FromMinutes(15));
            cache.Set(request.Email + "_timeout", DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 30, TimeSpan.FromSeconds(30));
            await EmailService.SendEmailAsync(request.Email, "Смена пароля", EmailService.CreatePasswordRecoveryEmailMessage(user.UserName, code));
            return new UnauthorizedObjectResult(new BaseResponse
            {
                Code = ResponseCodes.Unauthorized,
                Message = "Код выслан повторно"
            });
        }
        if (request.Code != email_code)
            return new UnauthorizedObjectResult(new BaseResponse { Code = ResponseCodes.ValidationError, Message = "Код не совпадает" });

        var token = await signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
        var result = await signInManager.UserManager.ResetPasswordAsync(user, token, request.NewPassword);
        if(!result.Succeeded)
        {
            var errors = result.Errors.Select(i => i.Description).ToList();
            throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, "Не удалось сменить пароль", errors);
        }

        await EmailService.SendEmailAsync(request.Email, "Пароль сменен", EmailService.CreatePasswordRecoveryNotificationMessage(user.UserName));
        return new OkObjectResult(new BaseResponse { Code = ResponseCodes.DataUpdated, Message = "Пароль успешно изменен" });
    }
}
