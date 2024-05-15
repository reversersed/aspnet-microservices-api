using Extensions;
using IdentityApi.src.Data.Entities;
using IdentityApi.src.InternalExtensions;
using IdentityApi.src.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ResponsePackage;

namespace IdentityApi.src.Handlers;

public class RegistrationHandler(SignInManager<User> _signInManager, ILogger<RegistrationHandler> _logger, IMemoryCache _cache) : IRequestHandler<RegistrationQuery, ActionResult<BaseResponse>>
{
    private readonly SignInManager<User> signInManager = _signInManager ?? throw new ArgumentNullException(nameof(_signInManager));
    private readonly ILogger<RegistrationHandler> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly IMemoryCache cache = _cache ?? throw new ArgumentNullException(nameof(_cache));

    public async Task<ActionResult<BaseResponse>> Handle(RegistrationQuery request, CancellationToken cancellationToken)
    {
        var check_user = await signInManager.UserManager.FindByNameAsync(request.Username);
        if (check_user is not null)
            return new ConflictObjectResult(new BaseResponse { Code = ResponseCodes.NotUnique, Message = "Имя пользователя уже занято" });
        check_user = await signInManager.UserManager.FindByEmailAsync(request.Email);
        if (check_user is not null)
            return new ConflictObjectResult(new BaseResponse { Code = ResponseCodes.NotUnique, Message = "Почта уже используется" });

        /*if (!cache.TryGetValue(request.Email + "_code", out int email_code))
        {
            var code = EmailService.GenerateNewCode();
            cache.Set(request.Email + "_code", code, TimeSpan.FromMinutes(15));
            cache.Set(request.Email + "_timeout", DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 30, TimeSpan.FromSeconds(30));
            await EmailService.SendEmailAsync(request.Email, "Подтверждение почты", EmailService.CreateEmailRegistrationConfirmationMessage(request.Username, code));
            logger.LogInformation("[IdentityApi] Sent email confirmation code to {email}", request.Email);
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
            await EmailService.SendEmailAsync(request.Email, "Подтверждение почты", EmailService.CreateEmailRegistrationConfirmationMessage(request.Username, code));
            logger.LogInformation("[IdentityApi] Sent repeated email confirmation code to {email}", request.Email);
            return new UnauthorizedObjectResult(new BaseResponse
            {
                Code = ResponseCodes.Unauthorized,
                Message = "Код выслан повторно"
            });
        }
        if (email_code != request.Code)
            throw new CustomExceptionResponse(ResponseCodes.ValidationError, "Код не совпадает");*/

        var user = new User { UserName = request.Username, Email = request.Email };
        var result = await signInManager.UserManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, "Во время регистрации произошла ошибка", result.Errors.Select(i => i.Description).ToList());
        await signInManager.UserManager.AddToRoleAsync(user, ClaimRoles.User);
        //add default scopes

        return new CreatedAtActionResult(null, null, null, new BaseResponse{ Code = ResponseCodes.DataCreated, Message = $"Пользователь {user.UserName} успешно зарегистирован" });
    }
}
