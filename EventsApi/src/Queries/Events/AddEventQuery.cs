using EventsApi.src.Data.Entities;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Events;

public class AddEventQuery : IRequest<ActionResult<BaseResponse<EventDTO>>>
{
    public long CategoryId { get; set; } = 0;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Creator { get; set; } = string.Empty;

    public string Speaker { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public DateTime StartDate { get; set; } = DateTime.MinValue;

    public int Seats { get; set; } = 0;
    public IFormFile? Image { get; set; }
}
    