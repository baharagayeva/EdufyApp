using Edufy.Application.Commons;
using Edufy.Application.CQRS.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdufyNew.Controllers;

[ApiController]
[Route("api/home")]
[Authorize]
public class HomeController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int popularTake = 5,
        [FromQuery] int instructorTake = 4,
        CancellationToken ct = default)
        => (await mediator.Send(new GetHomeQueryRequest(), ct)).ToActionResult();
}