using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace IdentityApi.src.Queries.Internal
{
    public class FindUserQuery : IRequest<ActionResult<BaseResponse<UserResponse>>>
    {
        public long Id { get; set; } = 0;
        public string UserName { get; set; } = "";
    }
}
