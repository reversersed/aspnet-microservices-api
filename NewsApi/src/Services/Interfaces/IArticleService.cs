using NewsApi.src.Services.Entities;

namespace NewsApi.src.Services.Interfaces;

public interface IArticleService
{
    Task<ArticleDTO> Get(long id);
    Task<ArticleDTO> Add(ArticleDTO article);
    Task<IEnumerable<ArticleDTO>> Get(int offset = 0, int count = 10);
    Task Delete(long id);
    Task<ArticleDTO> Patch(long id, string? Title, string? Content);
    Task ChangeUsername(string oldname, string newname);
}