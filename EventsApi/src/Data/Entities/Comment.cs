using System.ComponentModel.DataAnnotations;

namespace EventsApi.src.Data.Entities;

public class Comment
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public Creator Creator { get; set; }
    [Required]
    public Event Event { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
