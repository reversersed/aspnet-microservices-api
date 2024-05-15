using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Categories;

public class GetCategoryByIdQuery : IRequest<ActionResult<BaseResponse<CategoryDTO>>>
{
    public long Id { get; set; } = 0;
}
