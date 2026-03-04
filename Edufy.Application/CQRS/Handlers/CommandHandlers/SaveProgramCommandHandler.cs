using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Commands.Responses;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.Entities;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.CommandHandlers;

public sealed class SaveProgramCommandHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<SaveProgramCommandRequest, Result<SaveProgramCommandResponse>>
{
    public async Task<Result<SaveProgramCommandResponse>> Handle(
        SaveProgramCommandRequest request, CancellationToken ct)
    {
        var userId = currentUser.UserId;

        var programExists = await db.Programs
            .AnyAsync(x => x.Id == request.ProgramId, ct);

        if (!programExists)
            return Result<SaveProgramCommandResponse>.NotFound("Proqram not found.");

        var alreadySaved = await db.SavedPrograms
            .AnyAsync(x => x.UserId == userId && x.ProgramId == request.ProgramId, ct);

        if (alreadySaved)
            return Result<SaveProgramCommandResponse>.Conflict("This program is already saved.");

        var saved = new SavedProgram
        {
            UserId    = userId,
            ProgramId = request.ProgramId,
            SavedAt   = DateTime.UtcNow.AddHours(4)
        };

        db.SavedPrograms.Add(saved);
        await db.SaveChangesAsync(ct);

        return Result<SaveProgramCommandResponse>.Ok(new SaveProgramCommandResponse(saved.Id));
    }
}