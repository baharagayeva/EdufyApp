using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Commands.Responses;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.Entities;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.CommandHandlers;

public sealed class SaveLessonCommandHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<SaveLessonCommandRequest, Result<SaveLessonCommandResponse>>
{
    public async Task<Result<SaveLessonCommandResponse>> Handle(SaveLessonCommandRequest request, CancellationToken ct)
    {
        var userId = currentUser.UserId;

        var lessonExists = await db.Lessons
            .AnyAsync(x => x.Id == request.LessonId, ct);

        if (!lessonExists)
            return Result<SaveLessonCommandResponse>.NotFound("Lesson not found.");

        var alreadySaved = await db.SavedLessons
            .AnyAsync(x => x.UserId == userId && x.LessonId == request.LessonId, ct);

        if (alreadySaved)
            return Result<SaveLessonCommandResponse>.Conflict("This lesson is already saved.");

        var saved = new SavedLesson
        {
            UserId = userId,
            LessonId = request.LessonId,
            SavedAt = DateTime.UtcNow.AddHours(4)
        };

        db.SavedLessons.Add(saved);
        await db.SaveChangesAsync(ct);

        return Result<SaveLessonCommandResponse>.Ok(new SaveLessonCommandResponse(saved.Id));
    }
}