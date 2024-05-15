using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Queries;

public class ChangePasswordQuery : IRequest<ActionResult<BaseResponse>>
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
