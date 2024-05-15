using EventsApi.src.Data.Entities;
using EventsApi.src.Services.Entities;

namespace EventsApi.src.Repositories.Interfaces;

public interface ICommentRepository
{
    Task<Comment> Get(long id);
    Task<IEnumerable<Comment>> GetByEvent(long eventId, int offset = 0, int count = 0);
    Task Delete(long id);
    Task<Comment> Update(Comment comment);
    Task<Comment> Add(Comment comment, long eventId, long creatorId);
}
