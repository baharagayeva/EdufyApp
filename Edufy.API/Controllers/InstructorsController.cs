using Edufy.Application.Commons;
using Edufy.Application.CQRS.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdufyNew.Controllers;

[ApiController]
[Route("api/instructors")]
[Authorize]
public sealed class InstructorsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProfile(int id, [FromQuery] int demoTake, CancellationToken ct = default)
        => (await mediator.Send(new GetInstructorProfileQueryRequest(id, demoTake), ct)).ToActionResult();
}