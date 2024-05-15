namespace EventsApi.src.Services.Entities;

public class EventPreviewDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public string EventImage { get; set; } = string.Empty;
}
