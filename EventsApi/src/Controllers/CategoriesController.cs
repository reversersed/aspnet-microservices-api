using EventsApi.src.Queries.Categories;
using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController(IMediator _mediator) : ControllerBase
{
    private readonly IMediator mediator = _mediator ?? throw new ArgumentNullException(nameof(_mediator));

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<CategoryDTO>>> GetCategory(long id)
    {
        return await mediator.Send(new GetCategoryByIdQuery { Id = id });
    }
    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<CategoryDTO>>>> GetCategories([FromQuery] string filter = "all")
    {
        return await mediator.Send(new GetCategoriesQuery { Filter = filter });
    }
    [HttpPost]
    public async Task<ActionResult<BaseResponse<CategoryDTO>>> AddCategory([FromBody] AddCategoryQuery query)
    {
        return await mediator.Send(query);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<BaseResponse<CategoryDTO>>> UpdateCategory(long Id, [FromBody] UpdateCategoryQuery query)
    {
        query.Id = Id;
        return await mediator.Send(query);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse>> DeleteCategory(long Id)
    {
        return await mediator.Send(new DeleteCategoryQuery { Id = Id });
    }
}