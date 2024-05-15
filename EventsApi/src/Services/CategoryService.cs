using EventsApi.src.Data.Entities;
using EventsApi.src.Repositories.Interfaces;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using Mapster;

namespace EventsApi.src.Services;

public class CategoryService(ICategoryRepository _categoryRepository, IEventRepository _eventRepository) : ICategoryService
{
    private readonly ICategoryRepository categoryRepository = _categoryRepository ?? throw new ArgumentNullException(nameof(_categoryRepository));
    private readonly IEventRepository eventRepository = _eventRepository ?? throw new ArgumentNullException(nameof(_eventRepository));

    public async Task<CategoryDTO> Add(CategoryDTO category)
    {
        var entity = await categoryRepository.Add(category.Adapt<Category>());
        return entity.Adapt<CategoryDTO>();
    }

    public async Task Delete(long id)
    {
        var clear_images = await eventRepository.GetSavedImagePathsByCategory(id);
        clear_images.ForEach(image =>
        {
            if(image.Length > 0 && File.Exists(image))
                File.Delete(image);
        });
        await categoryRepository.Delete(id);
    }

    public async Task<CategoryDTO> Get(long id) => (await categoryRepository.Get(id)).Adapt<CategoryDTO>();

    public async Task<IEnumerable<CategoryDTO>> Get() => (await categoryRepository.Get()).Adapt<IEnumerable<CategoryDTO>>();
    public async Task<IEnumerable<CategoryDTO>> GetNotEmpty() => (await categoryRepository.GetNotEmpty()).Adapt<IEnumerable<CategoryDTO>>();

    public async Task<CategoryDTO> Update(CategoryDTO category)
    {
        var entity = await categoryRepository.Update(category.Adapt<Category>());
        return entity.Adapt<CategoryDTO>();
    }
}
