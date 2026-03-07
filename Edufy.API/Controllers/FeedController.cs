using Edufy.Application.Commons;
using Edufy.Application.CQRS.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdufyNew.Controllers;

[ApiController]
[Route("api/feed")]
[Authorize]
public sealed class FeedController(IMediator mediator) : ControllerBase
{
    [HttpGet("videos")]
    public async Task<IActionResult> GetVideos(
        [FromQuery] GetFeedVideosQueryRequest request,
        CancellationToken ct = default)
        => (await mediator.Send(request, ct)).ToActionResult();
    
    [HttpGet("videos/{lessonId:int}")]
    public async Task<IActionResult> GetVideoDetail(int lessonId, CancellationToken ct = default)
        => (await mediator.Send(new GetLessonDetailQueryRequest(lessonId), ct)).ToActionResult();
}