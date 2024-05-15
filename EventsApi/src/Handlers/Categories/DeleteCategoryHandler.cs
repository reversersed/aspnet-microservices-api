using EventsApi.src.Queries.Categories;
using EventsApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Handlers.Categories
{
    public class DeleteCategoryHandler(ICategoryService _categoryService) : IRequestHandler<DeleteCategoryQuery, ActionResult<BaseResponse>>
    {
        private readonly ICategoryService categoryService = _categoryService ?? throw new ArgumentNullException(nameof(_categoryService));

        public async Task<ActionResult<BaseResponse>> Handle(DeleteCategoryQuery request, CancellationToken cancellationToken)
        {
            await categoryService.Delete(request.Id);
            return new OkObjectResult(new BaseResponse { Code = ResponseCodes.DataDeleted, Message = "Категория удалена" });
        }
    }
}
