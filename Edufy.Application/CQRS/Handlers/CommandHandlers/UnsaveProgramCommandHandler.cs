using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Commands.Responses;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.CommandHandlers;

public sealed class UnsaveProgramCommandHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<UnsaveProgramCommandRequest, Result<UnsaveProgramCommandResponse>>
{
    public async Task<Result<UnsaveProgramCommandResponse>> Handle(UnsaveProgramCommandRequest request, CancellationToken ct)
    {
        var userId = currentUser.UserId;

        var saved = await db.SavedPrograms
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProgramId == request.ProgramId, ct);

        if (saved is null)
            return Result<UnsaveProgramCommandResponse>.NotFound("Saved program doesn't exist.");

        db.SavedPrograms.Remove(saved);
        await db.SaveChangesAsync(ct);

        return Result<UnsaveProgramCommandResponse>.Ok(new UnsaveProgramCommandResponse(true));
    }
}