using EventsApi.src.Data;
using EventsApi.src.Data.Entities;
using EventsApi.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ResponsePackage;

namespace EventsApi.src.Repositories;

public class CategoryRepository(DataContext _dataContext) : ICategoryRepository
{
    private readonly DataContext dataContext = _dataContext ?? throw new ArgumentNullException(nameof(_dataContext));

    public async Task<Category> Add(Category category)
    {
        var entity = await dataContext.Categories.AddAsync(category);
        await dataContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task Delete(long id)
    {
        var result = await dataContext.Categories.Where(x => x.Id == id).ExecuteDeleteAsync();
        if (result == 0)
            throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Категории не существует");
        await dataContext.SaveChangesAsync();
    }

    public async Task<Category> Get(long id)
    {
        return await dataContext.Categories.FindAsync(id) ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Категории не существует");
    }

    public async Task<IEnumerable<Category>> Get()
    {
        return await dataContext.Categories.OrderBy(x => x.Name).ToListAsync();
    }
    public async Task<IEnumerable<Category>> GetNotEmpty()
    {
        return await dataContext.Events.Where(x => x.StartDate > DateTime.UtcNow).Include(x => x.Category).Select(x => x.Category).Distinct().OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<Category> Update(Category category)
    {
        var entity = dataContext.Categories.Update(category);
        await dataContext.SaveChangesAsync();
        return entity.Entity;
    }
}
