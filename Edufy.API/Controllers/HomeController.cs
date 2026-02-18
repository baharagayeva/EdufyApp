using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Application.CQRS.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edufy.Controllers;

[ApiController]
[Route("api/home")]
[Authorize]
public class HomeController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<GetHomeQueryResponse> Get(CancellationToken ct)
        => await mediator.Send(new GetHomeQueryRequest(), ct);
}