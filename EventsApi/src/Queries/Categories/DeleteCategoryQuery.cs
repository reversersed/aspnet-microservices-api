
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Categories
{
    public class DeleteCategoryQuery : IRequest<ActionResult<BaseResponse>>
    {
        public long Id { get; set; } = 0;
    }
}
