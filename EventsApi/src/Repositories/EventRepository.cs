using EventsApi.src.Data;
using EventsApi.src.Data.Entities;
using EventsApi.src.Repositories.Interfaces;
using EventsApi.src.Services.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ResponsePackage;

namespace EventsApi.src.Repositories
{
    public class EventRepository(DataContext _context) : IEventRepository
    {
        private readonly DataContext context = _context ?? throw new ArgumentNullException(nameof(_context));
        public async Task<Event> Add(Event Event, long categoryId, long userId)
        {
            Event.Category = await context.Categories.FindAsync(categoryId) ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Указанной категории не существует");
            Event.Creator = await context.Creators.FindAsync(userId) ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Не удается определить создателя мероприятия");
            var entity = await context.Events.AddAsync(Event);
            await context.SaveChangesAsync();

            return entity.Entity;
        }

        public async Task Delete(long id)
        {
            int rows = await context.Events.Where(x => x.Id == id).ExecuteDeleteAsync();
            if (rows == 0)
                throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Указанного мероприятия не существует");
            await context.SaveChangesAsync();
        }

        public async Task<Event> Get(long id) => await context.Events.Where(x => x.Id == id).Include(x => x.Category).Include(x => x.Creator).SingleOrDefaultAsync() ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Указанного мероприятия не существует");

        public async Task<IEnumerable<Event>> Get(long categoryId, string sorting, int offset = 0, int count = 10)
        {
            return sorting switch
            {
                "" or "newest" => await context.Events
                                        .Where(x => x.StartDate > DateTime.UtcNow)
                                        .Include(x => x.Category)
                                        .Where(x => categoryId == 0 || x.Category.Id == categoryId)
                                        .OrderByDescending(x => x.Created)
                                        .Skip(offset).Take(count)
                                        .ToListAsync(),
                "nearest" => await context.Events
                                        .Where(x => x.StartDate > DateTime.UtcNow)
                                        .Include(x => x.Category)
                                        .Where(x => categoryId == 0 || x.Category.Id == categoryId)
                                        .OrderByDescending(x => DateTime.UtcNow - x.StartDate)
                                        .Skip(offset).Take(count)
                                        .ToListAsync(),
                _ => throw new CustomExceptionResponse(ResponseCodes.ValidationError, "Невалидный фильтр"),
            };
        }
        public async Task<List<string>> GetSavedImagePathsByCategory(long categoryId) => await context.Events.Include(x => x.Category).Where(x => x.Category.Id == categoryId).Where(x => x.EventImage.Length > 0).Select(x => x.EventImage).Distinct().ToListAsync();

        public async Task<Event> Update(EventDTO Event, long categoryId)
        {
            var EventData = await context.Events.Where(x => x.Id == Event.Id).Include(x => x.Category).SingleOrDefaultAsync() ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Указаного мероприятия не существует");
            EventData = Event.Adapt(EventData);
            EventData.Category = await context.Categories.FindAsync(categoryId) ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Не удается найти указанную категорию");
            EventData.Creator = await context.Creators.Where(x => x.Username.Equals(Event.Creator)).SingleOrDefaultAsync() ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Не удается найти создателя мероприятия");

            var entity = context.Events.Update(EventData);
            await context.SaveChangesAsync();
            return entity.Entity;
        }
    }
}
