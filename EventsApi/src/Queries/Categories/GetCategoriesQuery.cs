using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Categories;

public class GetCategoriesQuery : IRequest<ActionResult<BaseResponse<IEnumerable<CategoryDTO>>>>
{
    public string Filter { get; set; } = "all";
}
