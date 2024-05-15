using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Queries;

public class RevokeQuery : IRequest<ActionResult<BaseResponse>>
{
}
