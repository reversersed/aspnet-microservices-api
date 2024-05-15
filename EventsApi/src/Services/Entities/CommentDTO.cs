namespace EventsApi.src.Services.Entities;

public class CommentDTO
{
    public long Id { get; set; }
    public string Content { get; set; }
    public string Creator { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
