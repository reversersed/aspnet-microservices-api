using EventsApi.src.Data.Entities;

namespace EventsApi.src.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<Category> Get(long id);
    Task<IEnumerable<Category>> Get();
    Task<IEnumerable<Category>> GetNotEmpty();
    Task<Category> Add(Category category);
    Task<Category> Update(Category category);
    Task Delete(long id);
}
