using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResponsePackage;

namespace EventsApi.src.Queries.Events;

public class DeleteEventQuery : IRequest<ActionResult<BaseResponse>>
{
    public long Id { get; set; }
}
