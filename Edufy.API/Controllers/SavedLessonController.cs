using Edufy.Application.Commons;
using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdufyNew.Controllers;

[ApiController]
[Route("api/saved-lessons")]
[Authorize]
public class SavedLessonController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SaveLessonCommandRequest request, CancellationToken ct)
        => (await mediator.Send(request, ct)).ToActionResult();

    [HttpDelete("{lessonId:int}")]
    public async Task<IActionResult> Unsave(int lessonId, CancellationToken ct)
        => (await mediator.Send(new UnsaveLessonCommandRequest(lessonId), ct)).ToActionResult();

    [HttpGet("my")]
    public async Task<IActionResult> GetMySavedLessons(CancellationToken ct)
        => (await mediator.Send(new GetMySavedLessonsQueryRequest(), ct)).ToActionResult();
}