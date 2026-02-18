using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Application.CQRS.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edufy.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<GetCoursesQueryResponse>> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int take = 20, CancellationToken ct = default)
        => await mediator.Send(new GetCoursesQueryRequest(search, page, take), ct);

    [HttpGet("{id:int}")]
    public async Task<GetCourseDetailQueryResponse> GetById(int id, CancellationToken ct)
        => await mediator.Send(new GetCourseDetailQueryRequest(id), ct);

    [HttpPost("{id:int}/apply")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Apply(int id, CancellationToken ct)
    {
        var applicationId = await mediator.Send(new ApplyToCourseCommandRequest(id), ct);
        return Ok(new { ApplicationId = applicationId });
    }
}