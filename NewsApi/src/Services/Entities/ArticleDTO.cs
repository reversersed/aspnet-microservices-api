using NewsApi.src.Data.Entities;

namespace NewsApi.src.Services.Entities;

public class ArticleDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public string Author { get; set; }
}