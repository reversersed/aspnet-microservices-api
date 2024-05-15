using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Categories
{
    public class AddCategoryQuery : IRequest<ActionResult<BaseResponse<CategoryDTO>>>
    {
        public string Name { get; set; } = "";
    }
}
