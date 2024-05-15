using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Queries;

public class RegistrationQuery : IRequest<ActionResult<BaseResponse>>
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
    public int Code { get; set; } = 0;
}
