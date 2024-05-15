using EventsApi.src.Services.Entities;

namespace EventsApi.src.Services.Interfaces;

public interface IEventService
{
    Task<EventDTO> Get(long id);
    Task<IEnumerable<EventDTO>> Get(long categoryId = 0, string sorting = "", int offset = 0, int count = 10);
    Task<IEnumerable<EventPreviewDTO>> GetPreview(long categoryId = 0, string sorting = "", int offset = 0, int count = 10);
    Task<EventDTO> Add(EventDTO Event, long categoryId);
    Task<EventDTO> Update(EventDTO Event, long categoryId);
    Task Delete(long id);
}
