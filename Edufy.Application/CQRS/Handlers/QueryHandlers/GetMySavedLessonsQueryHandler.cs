using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.SavedLessonDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetMySavedLessonsQueryHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<GetMySavedLessonsQueryRequest, Result<List<SavedLessonDto>>>
{
    public async Task<Result<List<SavedLessonDto>>> Handle(GetMySavedLessonsQueryRequest request, CancellationToken ct)
    {
        var userId = currentUser.UserId;

        var saved = await db.SavedLessons
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.SavedAt)
            .Select(x => new SavedLessonDto(
                x.Id,
                x.LessonId,
                x.Lesson.Name,
                x.Lesson.ThumbnailUrl,
                x.Lesson.DurationMinutes,
                x.Lesson.VideoUrl,
                x.Lesson.Module.ProgramId,
                x.Lesson.Module.Program.Name,
                x.Lesson.Module.Program.InstructorId,
                x.Lesson.Module.Program.Instructor.FirstName + " " + x.Lesson.Module.Program.Instructor.LastName,
                x.SavedAt
            ))
            .ToListAsync(ct);

        return Result<List<SavedLessonDto>>.Ok(saved);
    }
}