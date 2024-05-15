using EventsApi.src.Queries.Categories;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Categories;

public class GetCategoriesHandler(ICategoryService _categoryService, ILogger<GetCategoryByIdHandler> _logger) : IRequestHandler<GetCategoriesQuery, ActionResult<BaseResponse<IEnumerable<CategoryDTO>>>>
{
    private readonly ICategoryService categoryService = _categoryService ?? throw new ArgumentNullException(nameof(_categoryService));
    private readonly ILogger<GetCategoryByIdHandler> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    public async Task<ActionResult<BaseResponse<IEnumerable<CategoryDTO>>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<CategoryDTO> result = request.Filter switch
        {
            "all" => await categoryService.Get(),
            "notempty" => await categoryService.GetNotEmpty(),
            _ => throw new CustomExceptionResponse(ResponseCodes.ValidationError, "Указаны неправильные фильтры"),
        };
        logger.LogInformation("[Events] Loaded {total} categories from database", result.Count());
        return new OkObjectResult(new BaseResponse<IEnumerable<CategoryDTO>> { Code = ResponseCodes.DataFound, Message = $"Найдено {result.Count()} категорий", Data = result });
    }
}
