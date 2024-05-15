using EventsApi.src.Queries.Categories;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Categories
{
    public class AddCategoryHandler(ICategoryService _categoryService, ILogger<AddCategoryHandler> _logger) : IRequestHandler<AddCategoryQuery, ActionResult<BaseResponse<CategoryDTO>>>
    {
        private readonly ICategoryService categoryService = _categoryService ?? throw new ArgumentNullException(nameof(_categoryService));
        private readonly ILogger<AddCategoryHandler> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));

        public async Task<ActionResult<BaseResponse<CategoryDTO>>> Handle(AddCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = await categoryService.Add(request.Adapt<CategoryDTO>());

            logger.LogInformation("[Events] Added new category: {categoryname} with Id {id}", result.Name, result.Id);
            return new CreatedAtActionResult(null, null, null, new BaseResponse<CategoryDTO> { Code = ResponseCodes.DataCreated, Message = $"Категория {result.Name} создана", Data = result });
        }
    }
}
