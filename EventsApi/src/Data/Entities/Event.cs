using System.ComponentModel.DataAnnotations;

namespace EventsApi.src.Data.Entities;

public class Event
{
    [Key]
    public long Id { get; set; }

    [Required]
    public Category Category { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; } = string.Empty;
    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public Creator Creator { get; set; } = new();

    public string Speaker { get; set; } = string.Empty;

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime StartDate { get; set; } = DateTime.MinValue;

    public int Seats { get; set; } = 0;

    public string EventImage { get; set; } = string.Empty;
}
