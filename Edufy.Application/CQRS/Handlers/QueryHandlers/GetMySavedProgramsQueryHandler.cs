using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.SavedProgramDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetMySavedProgramsQueryHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<GetMySavedProgramsQueryRequest, Result<List<SavedProgramDto>>>
{
    public async Task<Result<List<SavedProgramDto>>> Handle(
        GetMySavedProgramsQueryRequest request, CancellationToken ct)
    {
        var userId = currentUser.UserId;

        var saved = await db.SavedPrograms
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.SavedAt)
            .Select(x => new SavedProgramDto(
                x.Id,
                x.ProgramId,
                x.Program.Name,
                x.Program.Academy.Name,
                x.Program.Academy.LogoUrl,
                x.Program.DurationMonths + " ay",
                $"{x.Program.GroupMinSize}-{x.Program.GroupMaxSize} nəfər",
                x.SavedAt
            ))
            .ToListAsync(ct);

        return Result<List<SavedProgramDto>>.Ok(saved);
    }
}
