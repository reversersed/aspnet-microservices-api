using EventsApi.src.Data.Entities;
using EventsApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Events;

public class UpdateEventQuery : IRequest<ActionResult<BaseResponse<EventDTO>>>
{
    public long Id { get; set; } = 0;

    public long? CategoryId { get; set; } = null;

    public string? Title { get; set; } = null;

    public string? Description { get; set; } = null;

    public string? Speaker { get; set; } = null;

    public DateTime? StartDate { get; set; } = null;
    public string? Location { get; set; } = null;

    public int? Seats { get; set; } = null;
    public IFormFile? Image { get; set; } = null;
}
