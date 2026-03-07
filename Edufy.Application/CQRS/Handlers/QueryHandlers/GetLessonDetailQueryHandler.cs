using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.VideoCardDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetLessonDetailQueryHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<GetLessonDetailQueryRequest, Result<FeedVideoCardDto>>
{
    public async Task<Result<FeedVideoCardDto>> Handle(
        GetLessonDetailQueryRequest request, CancellationToken ct)
    {
        var userId = currentUser.UserId;

        var lesson = await db.Lessons
            .AsNoTracking()
            .Where(l => l.Id == request.LessonId && l.IsDemo && l.VideoUrl != null && l.VideoUrl != "")
            .Select(l => new FeedVideoCardDto(
                l.Id,
                l.Name,
                l.ThumbnailUrl,
                l.DurationMinutes,
                l.VideoUrl!,
                l.Module.Program.InstructorId,
                l.Module.Program.Instructor.FirstName + " " + l.Module.Program.Instructor.LastName,
                l.Module.ProgramId,
                l.Module.Program.Name,
                userId != Guid.Empty &&
                db.SavedLessons.Any(sl => sl.UserId == userId && sl.LessonId == l.Id)
            ))
            .FirstOrDefaultAsync(ct);

        if (lesson is null)
            return Result<FeedVideoCardDto>.NotFound("Lesson not found");

        return Result<FeedVideoCardDto>.Ok(lesson);
    }
}
