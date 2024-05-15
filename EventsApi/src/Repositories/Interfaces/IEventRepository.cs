using EventsApi.src.Data.Entities;
using EventsApi.src.Services.Entities;

namespace EventsApi.src.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<Event> Get(long id);
        Task<IEnumerable<Event>> Get(long categoryId = 0, string sorting = "", int offset = 0, int count = 10);
        Task<Event> Add(Event Event, long categoryId, long userId);
        Task<Event> Update(EventDTO Event, long categoryId);
        Task<List<string>> GetSavedImagePathsByCategory(long categoryId);
        Task Delete(long id);
    }
}