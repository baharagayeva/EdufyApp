using Edufy.Application.Commons;
using Edufy.Application.CQRS.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdufyNew.Controllers;

[ApiController]
[Route("api/academies")]
[Authorize]
public class AcademyController(IMediator mediator) : ControllerBase
{
    [HttpGet("{academyId:int}")]
    public async Task<IActionResult> GetAcademyDetail(int academyId, CancellationToken ct)
    => (await mediator.Send(new GetAcademyDetailQueryRequest(academyId), ct)).ToActionResult();
}