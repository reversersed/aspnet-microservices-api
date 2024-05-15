using IdentityApi.src.Queries.Internal;
using IdentityApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Handlers.Internal;

public class FindUserHandler(IUserService _userService, ILogger<FindUserHandler> _logger) : IRequestHandler<FindUserQuery, ActionResult<BaseResponse<UserResponse>>>
{
    private readonly ILogger<FindUserHandler> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    private readonly IUserService userService = _userService ?? throw new ArgumentNullException(nameof(_userService));

    public async Task<ActionResult<BaseResponse<UserResponse>>> Handle(FindUserQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("[FindUser] Requested user with Id {id} or name {user}", request.Id, request.UserName);
        UserResponse user = request.Id > 0 ? await userService.GetUserById((long)request.Id) : await userService.GetUserByUsername(request.UserName);

        return new OkObjectResult(new BaseResponse<UserResponse> { Code = ResponseCodes.DataFound, Message = "Пользователь найден", Data = user });
    }
}
