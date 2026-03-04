using Edufy.Application.Commons;
using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdufyNew.Controllers;

[ApiController]
[Route("api/saved-programs")]
[Authorize]
public class SavedProgramController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SaveProgramCommandRequest request, CancellationToken ct)
    => (await mediator.Send(request, ct)).ToActionResult();

    [HttpDelete("{programId:int}")]
    public async Task<IActionResult> Unsave(int programId, CancellationToken ct)
    => (await mediator.Send(new UnsaveProgramCommandRequest(programId), ct)).ToActionResult();

    [HttpGet("my")]
    public async Task<IActionResult> GetMySavedPrograms(CancellationToken ct)
    => (await mediator.Send(new GetMySavedProgramsQueryRequest(), ct)).ToActionResult();
}