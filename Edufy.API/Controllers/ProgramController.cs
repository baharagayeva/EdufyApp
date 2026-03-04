using Edufy.Application.Commons;
using Edufy.Application.CQRS.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdufyNew.Controllers;

[ApiController]
[Route("api/programs")]
[Authorize]
public class ProgramController(IMediator mediator) : ControllerBase
{
    [HttpGet("{programId:int}")]
    public async Task<IActionResult> GetProgramDetail(int programId, CancellationToken ct)
    => (await mediator.Send(new GetProgramDetailQueryRequest(programId), ct)).ToActionResult();
}