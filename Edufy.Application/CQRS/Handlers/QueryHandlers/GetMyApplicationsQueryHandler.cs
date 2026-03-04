using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.ApplicationDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetMyApplicationsQueryHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<GetMyApplicationsQueryRequest, Result<List<MyApplicationDto>>>
{
    public async Task<Result<List<MyApplicationDto>>> Handle(
        GetMyApplicationsQueryRequest request, CancellationToken ct)
    {
        var applications = await db.Applications
            .AsNoTracking()
            .Where(x => x.UserId == currentUser.UserId)
            .OrderByDescending(x => x.AppliedAt)
            .Select(x => new MyApplicationDto(
                x.Id,
                x.ProgramId,
                x.Program.Name,
                x.Program.Academy.Name,
                x.Program.Academy.LogoUrl,
                x.Program.DurationMonths + " ay",
                x.Status,
                x.AppliedAt
            ))
            .ToListAsync(ct);

        return Result<List<MyApplicationDto>>.Ok(applications);
    }
}