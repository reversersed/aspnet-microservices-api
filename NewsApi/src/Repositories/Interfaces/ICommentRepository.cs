using NewsApi.src.Data.Entities;

namespace NewsApi.src.Repositories.Interfaces;

public interface ICommentRepository
{
    Task<Comment> Get(long id);
    Task<Comment> Add(long article_id, Comment comment);
    Task<IEnumerable<Comment>> Get(long article_id, int offset = 0, int count = 10);
    Task Delete(long id);
    Task<Comment> Patch(Comment comment);
    Task ChangeUsername(string oldname, string newname);
}