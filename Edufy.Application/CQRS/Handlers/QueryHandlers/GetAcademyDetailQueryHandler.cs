using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.AcademyDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetAcademyDetailQueryHandler(EdufyDbContext db)
    : IRequestHandler<GetAcademyDetailQueryRequest, Result<GetAcademyDetailQueryResponse>>
{
    public async Task<Result<GetAcademyDetailQueryResponse>> Handle(
        GetAcademyDetailQueryRequest request, CancellationToken ct)
    {
        var academy = await db.Academies
            .AsNoTracking()
            .Where(x => x.Id == request.AcademyId)
            .Select(x => new GetAcademyDetailQueryResponse(
                x.Id,
                x.Name,
                x.LogoUrl,
                x.About,
                x.TotalApplications,
                x.TotalStudents,
                x.GraduationRate,
                x.Programs
                    .OrderByDescending(p => p.Id)
                    .Select(p => new ProgramCardDto(
                        p.Id,
                        p.Name,
                        p.DurationMonths + " ay",
                        p.Status,
                        p.Instructor.PhotoUrl
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync(ct);

        if (academy is null)
            return Result<GetAcademyDetailQueryResponse>.NotFound("Academy not found.");

        return Result<GetAcademyDetailQueryResponse>.Ok(academy);
    }
}