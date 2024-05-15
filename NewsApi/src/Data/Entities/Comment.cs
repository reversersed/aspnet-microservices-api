using System.ComponentModel.DataAnnotations;

namespace NewsApi.src.Data.Entities;

public class Comment
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string Content { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    [Required]
    public Article Article { get; set; }
    [Required]
    public long AuthorId { get; set; }
    [Required]
    public string Author { get; set; }
}