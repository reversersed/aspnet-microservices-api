using EventsApi.src.Services.Entities;

namespace EventsApi.src.Services.Interfaces;

public interface ICommentService
{
    Task<CommentDTO> Get(long id);
    Task<IEnumerable<CommentDTO>> GetByEvent(long eventId, int offset = 0, int count = 0);
    Task Delete(long id);
    Task<CommentDTO> Update(CommentDTO comment);
    Task<CommentDTO> Add(CommentDTO comment, long eventId);
}
