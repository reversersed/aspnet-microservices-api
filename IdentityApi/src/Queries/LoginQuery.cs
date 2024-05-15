using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Queries;

public class LoginQuery : IRequest<ActionResult<BaseResponse<AuthenticationResponse>>>
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool RememberMe { get; set; } = false;
}
