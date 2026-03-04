using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.ProgramDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetProgramDetailQueryHandler(EdufyDbContext db)
    : IRequestHandler<GetProgramDetailQueryRequest, Result<GetProgramDetailQueryResponse>>
{
    public async Task<Result<GetProgramDetailQueryResponse>> Handle(
        GetProgramDetailQueryRequest request, CancellationToken ct)
    {
        var program = await db.Programs
            .AsNoTracking()
            .Where(x => x.Id == request.ProgramId)
            .Select(x => new GetProgramDetailQueryResponse(
                x.Id,
                x.Name,
                x.About,
                x.DurationMonths + " ay",
                $"{x.GroupMinSize}-{x.GroupMaxSize} nəfər",
                x.Modules
                    .OrderBy(m => m.Order)
                    .Select(m => new ModuleDto(
                        m.Id,
                        m.Order,
                        m.Name,
                        m.IsOpen,
                        m.Lessons
                            .OrderBy(l => l.Order)
                            .Select(l => new LessonDto(
                                l.Id,
                                l.Order,
                                l.Name,
                                l.DurationMinutes
                            ))
                            .ToList()
                    ))
                    .ToList(),
                new ProgramInstructorDto(
                    x.Instructor.Id,
                    x.Instructor.FirstName + " " + x.Instructor.LastName,
                    x.Instructor.Specialization,
                    x.Instructor.Bio,
                    x.Instructor.PhotoUrl,
                    x.Instructor.LinkedInUrl
                )
            ))
            .FirstOrDefaultAsync(ct);

        if (program is null)
            return Result<GetProgramDetailQueryResponse>.NotFound("Proqram not found.");

        return Result<GetProgramDetailQueryResponse>.Ok(program);
    }
}