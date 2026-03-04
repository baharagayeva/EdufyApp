using Edufy.Application.Commons;
using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdufyNew.Controllers;

[ApiController]
[Route("api/applications")]
[Authorize]
public class ApplicationController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Apply([FromBody] ApplyCommandRequest request, CancellationToken ct)
    => (await mediator.Send(request, ct)).ToActionResult();

    [HttpGet("my")]
    public async Task<IActionResult> GetMyApplications(CancellationToken ct)
    => (await mediator.Send(new GetMyApplicationsQueryRequest(), ct)).ToActionResult();
}