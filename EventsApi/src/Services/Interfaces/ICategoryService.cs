using EventsApi.src.Services.Entities;

namespace EventsApi.src.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryDTO> Get(long id);
    Task<IEnumerable<CategoryDTO>> Get();
    Task<IEnumerable<CategoryDTO>> GetNotEmpty();
    Task<CategoryDTO> Add(CategoryDTO category);
    Task<CategoryDTO> Update(CategoryDTO category);
    Task Delete(long id);
}
