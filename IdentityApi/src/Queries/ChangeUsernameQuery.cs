using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Queries;

public class ChangeUsernameQuery : IRequest<ActionResult<BaseResponse<AuthenticationResponse>>>
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}
