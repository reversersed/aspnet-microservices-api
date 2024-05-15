using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Queries;

public class RefreshQuery : IRequest<ActionResult<BaseResponse<AuthenticationResponse>>>
{
    public string Token { get; set; } = "";
    public string RefreshToken { get; set; } = "";
}
