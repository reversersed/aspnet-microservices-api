using EventsApi.src.Queries.Categories;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Categories;

public class GetCategoryByIdHandler(ICategoryService _categoryService, ILogger<GetCategoryByIdHandler> _logger) : IRequestHandler<GetCategoryByIdQuery, ActionResult<BaseResponse<CategoryDTO>>>
{
    private readonly ICategoryService categoryService = _categoryService ?? throw new ArgumentNullException(nameof(_categoryService));
    private readonly ILogger<GetCategoryByIdHandler> logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
    public async Task<ActionResult<BaseResponse<CategoryDTO>>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryService.Get(request.Id);

        logger.LogInformation("[Events] Category loaded from database: {categoryname} with Id {id}", result.Name, result.Id);
        return new OkObjectResult(new BaseResponse<CategoryDTO> { Code = ResponseCodes.DataFound, Message = $"Найдена категория {result.Name}", Data = result });
    }
}
