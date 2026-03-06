using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Commands.Responses;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.CommandHandlers;

public sealed class UnsaveLessonCommandHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<UnsaveLessonCommandRequest, Result<UnsaveLessonCommandResponse>>
{
    public async Task<Result<UnsaveLessonCommandResponse>> Handle(UnsaveLessonCommandRequest request, CancellationToken ct)
    {
        var userId = currentUser.UserId;

        var saved = await db.SavedLessons
            .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == request.LessonId, ct);

        if (saved is null)
            return Result<UnsaveLessonCommandResponse>.NotFound("Saved lesson doesn't exist.");

        db.SavedLessons.Remove(saved);
        await db.SaveChangesAsync(ct);

        return Result<UnsaveLessonCommandResponse>.Ok(new UnsaveLessonCommandResponse(true));
    }
}