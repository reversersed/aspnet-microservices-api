using NewsApi.src.Data.Entities;

namespace NewsApi.src.Repositories.Interfaces;

public interface IArticleRepository
{
    Task<Article> Get(long id);
    Task<Article> Add(Article article);
    Task<IEnumerable<Article>> Get(int offset = 0, int count = 10);
    Task Delete(long id);
    Task<Article> Patch(Article article);
    Task ChangeUsername(string oldname, string newname);
}