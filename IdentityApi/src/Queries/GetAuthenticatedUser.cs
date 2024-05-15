using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Queries;

public class GetAuthenticatedUser : IRequest<ActionResult<BaseResponse<UserResponse>>>
{
}
