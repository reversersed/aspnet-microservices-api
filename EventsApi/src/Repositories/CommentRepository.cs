using EventsApi.src.Data;
using EventsApi.src.Data.Entities;
using EventsApi.src.Repositories.Interfaces;
using EventsApi.src.Services.Entities;
using Microsoft.EntityFrameworkCore;
using ResponsePackage;

namespace EventsApi.src.Repositories;

public class CommentRepository(DataContext _dataContext) : ICommentRepository
{
    private readonly DataContext dataContext = _dataContext ?? throw new ArgumentNullException(nameof(dataContext));
    public async Task<Comment> Add(Comment comment, long eventId, long creatorId)
    {
        comment.Creator = await dataContext.Creators.FindAsync(creatorId) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Не удается определить создателя комментария");
        comment.Event = await dataContext.Events.FindAsync(eventId) ?? throw new CustomExceptionResponse(ResponseCodes.UserNotFound, "Не удается определить мероприятие");
        var entity = await dataContext.Comments.AddAsync(comment);
        await dataContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task Delete(long id)
    {
        int num = await dataContext.Comments.Where(x => x.Id == id).ExecuteDeleteAsync();
        if (num == 0)
            throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Комментарий не найден");
        await dataContext.SaveChangesAsync();
    }

    public async Task<Comment> Get(long id) => await dataContext.Comments.Where(x => x.Id == id).Include(x => x.Creator).SingleOrDefaultAsync() ?? throw new CustomExceptionResponse(ResponseCodes.ObjectNotFound, "Комментарий не найден");

    public async Task<IEnumerable<Comment>> GetByEvent(long eventId, int offset = 0, int count = 0) => await dataContext.Comments.Include(x => x.Event).Where(x => x.Event.Id == eventId).Include(x => x.Creator).OrderByDescending(e => e.Created).Skip(offset).Take(count).ToListAsync();

    public async Task<Comment> Update(Comment comment)
    {
        var entity = dataContext.Comments.Update(comment);
        await dataContext.SaveChangesAsync();
        return entity.Entity;
    }
}
