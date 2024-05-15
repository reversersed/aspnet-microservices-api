using NewsApi.src.Services.Entities;

namespace NewsApi.src.Services.Interfaces;

public interface ICommentService
{
    Task<CommentDTO> Get(long id);
    Task<CommentDTO> Add(long article_id, CommentDTO comment);
    Task<IEnumerable<CommentDTO>> Get(long article_id, int offset = 0, int count = 10);
    Task Delete(long id);
    Task<CommentDTO> Patch(long id, string? Content);
    Task ChangeUsername(string oldname, string newname);
}