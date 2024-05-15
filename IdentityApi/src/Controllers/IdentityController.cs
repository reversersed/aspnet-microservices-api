using IdentityApi.src.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Controllers;

[Route("[controller]")]
[ApiController]
public class IdentityController(IMediator _mediator) : ControllerBase
{
    private readonly IMediator mediator = _mediator ?? throw new ArgumentNullException(nameof(_mediator));

    [HttpPost("login")]
    public async Task<ActionResult<BaseResponse<AuthenticationResponse>>> Login([FromBody] LoginQuery query)
    {
        return await mediator.Send(query);
    }
    [HttpPost("revoke")]
    public async Task<ActionResult<BaseResponse>> Revoke()
    {
        return await mediator.Send(new RevokeQuery());
    }
    [HttpPost("refresh")]
    public async Task<ActionResult<BaseResponse<AuthenticationResponse>>> Refresh([FromBody] RefreshQuery query)
    {
        return await mediator.Send(query);
    }
    [HttpPost("rename")]
    public async Task<ActionResult<BaseResponse<AuthenticationResponse>>> Rename([FromBody] ChangeUsernameQuery query)
    {
        return await mediator.Send(query);
    }
    [HttpPost("register")]
    public async Task<ActionResult<BaseResponse>> Registration([FromBody] RegistrationQuery query)
    {
        return await mediator.Send(query);
    }
    [HttpPost("recovery")]
    public async Task<ActionResult<BaseResponse>> RecoveryPassword([FromBody] RecoveryPasswordQuery query)
    {
        return await mediator.Send(query);
    }
    [HttpPost("changepassword")]
    public async Task<ActionResult<BaseResponse>> ChangePassword([FromBody] ChangePasswordQuery query)
    {
        return await mediator.Send(query);
    }
    [HttpGet]
    public async Task<ActionResult<BaseResponse<UserResponse>>> GetUserByAuth()
    {
        return await mediator.Send(new GetAuthenticatedUser { });
    }
}
