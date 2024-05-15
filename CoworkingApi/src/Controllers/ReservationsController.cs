using CoworkingApi.src.Queries.Reservations;
using CoworkingApi.src.Services.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace CoworkingApi.src.Controllers;
[Route("[controller]")]
[ApiController]
public class ReservationsController(IMediator _mediator) : ControllerBase
{
    private readonly IMediator mediator = _mediator ?? throw new ArgumentNullException(nameof(mediator));
}
