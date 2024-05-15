using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApi.src.Data.Entities;

public class Article
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    [Required]
    public long AuthorId { get; set; }
    [Required]
    public string Author { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}