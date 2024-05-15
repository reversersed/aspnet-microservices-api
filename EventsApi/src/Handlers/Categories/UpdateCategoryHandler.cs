using EventsApi.src.Queries.Categories;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Categories
{
    public class UpdateCategoryHandler(ICategoryService _categoryService) : IRequestHandler<UpdateCategoryQuery, ActionResult<BaseResponse<CategoryDTO>>>
    {
        private readonly ICategoryService categoryService = _categoryService ?? throw new ArgumentNullException(nameof(_categoryService));
        public async Task<ActionResult<BaseResponse<CategoryDTO>>> Handle(UpdateCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = await categoryService.Update(request.Adapt<CategoryDTO>());
            return new OkObjectResult(new BaseResponse<CategoryDTO> { Code = ResponseCodes.DataUpdated, Message = "Категория обновлена", Data =  result });
        }
    }
}
