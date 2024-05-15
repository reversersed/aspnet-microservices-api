using IdentityApi.src.Queries.Internal;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;
using System.Xml.Linq;

namespace IdentityApi.src.Controllers;

[Route("[controller]")]
[ApiController]
public class InternalController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("byid")]
    public async Task<ActionResult<BaseResponse<UserResponse>>> GetUserById([FromQuery] long id)
    {
        return await mediator.Send(new FindUserQuery { Id = id });
    }
    [HttpGet("byname")]
    public async Task<ActionResult<BaseResponse<UserResponse>>> GetUserByName([FromQuery] string name)
    {
        return await mediator.Send(new FindUserQuery { UserName = name });
    }
}
