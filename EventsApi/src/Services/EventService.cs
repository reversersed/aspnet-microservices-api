using EventsApi.src.Data.Entities;
using EventsApi.src.Repositories.Interfaces;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using Mapster;
using ResponsePackage;
using Extensions.HttpExtension.Clients;
using Extensions.HttpExtension.Interfaces;

namespace EventsApi.src.Services;

public class EventService(IEventRepository _eventRepository, IInternalHttpClient _httpClient, ICreatorRepository _creatorRepository) : IEventService
{
    private readonly IEventRepository eventRepository = _eventRepository ?? throw new ArgumentNullException(nameof(_eventRepository));
    private readonly IInternalHttpClient httpClient = _httpClient ?? throw new ArgumentNullException(nameof(_httpClient));
    private readonly ICreatorRepository creatorRepository = _creatorRepository ?? throw new ArgumentNullException(nameof(_creatorRepository));
    public async Task<EventDTO> Add(EventDTO Event, long categoryId)
    {
        var data = Event.Adapt<Event>();

        var response = await httpClient.SendGet<UserResponse>("identityapi", "byname", ("name", Event.Creator));
        if (response.Code != ResponseCodes.DataFound || response.Data is null)
            throw new CustomExceptionResponse(ResponseCodes.UserNotFound, response.Message ?? string.Empty);
        await creatorRepository.TryAddCreator(response.Data.Id, response.Data.UserName);

        var entity = await eventRepository.Add(data, categoryId, response.Data.Id);
        return entity.Adapt<EventDTO>();
    }

    public async Task Delete(long id)
    {
        var Event = await eventRepository.Get(id);
        if(Event.EventImage.Length > 0 && File.Exists(Event.EventImage))
            File.Delete(Event.EventImage);
        await eventRepository.Delete(id);
    }

    public async Task<EventDTO> Get(long id) => (await eventRepository.Get(id)).Adapt<EventDTO>();

    public async Task<IEnumerable<EventDTO>> Get(long categoryId = 0, string sorting = "", int offset = 0, int count = 10)
    {
        var list = await eventRepository.Get(categoryId, sorting, offset, count);
        if (!list.Any())
            throw new CustomExceptionResponse(ResponseCodes.EmptySequence, "Получена пустая коллекция");

        return list.Adapt<IEnumerable<EventDTO>>();
    }

    public async Task<IEnumerable<EventPreviewDTO>> GetPreview(long categoryId = 0, string sorting = "", int offset = 0, int count = 10)
    {
        var list = await eventRepository.Get(categoryId, sorting, offset, count);
        if (!list.Any())
            throw new CustomExceptionResponse(ResponseCodes.EmptySequence, "Получена пустая коллекция");

        return list.Adapt<IEnumerable<EventPreviewDTO>>();
    }

    public async Task<EventDTO> Update(EventDTO Event, long categoryId)
    {
        return (await eventRepository.Update(Event, categoryId)).Adapt<EventDTO>();
    }
}
