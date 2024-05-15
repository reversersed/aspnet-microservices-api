using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Queries;

public class RecoveryPasswordQuery : IRequest<ActionResult<BaseResponse>>
{
    public string Email { get; set; } = "";
    public string NewPassword { get; set; } = "";
    public int Code { get; set; } = 0;
}
