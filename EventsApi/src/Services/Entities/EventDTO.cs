using System.Text.Json.Serialization;
using EventsApi.src.Data.Entities;

namespace EventsApi.src.Services.Entities;

public class EventDTO
{
    public long Id { get; set; }

    public CategoryDTO Category { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Creator { get; set; }

    public string Speaker { get; set; }
    public string Location { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime StartDate { get; set; }

    public int Seats { get; set; } = 0;
    public string EventImage { get; set; } = string.Empty;
}
