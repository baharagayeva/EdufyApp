using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.HomeDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetHomeQueryHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<GetHomeQueryRequest, Result<GetHomeQueryResponse>>
{
    public async Task<Result<GetHomeQueryResponse>> Handle(GetHomeQueryRequest request, CancellationToken ct)
    {
        var academies = await db.Academies
            .AsNoTracking()
            .OrderByDescending(x => x.TotalStudents)
            .Take(request.PopularTake)
            .Select(x => new AcademyCardDto(
                x.Id,
                x.Name,
                x.LogoUrl,
                x.Programs.Count
            ))
            .ToListAsync(ct);

        var instructors = await db.Instructors
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Take(request.InstructorTake)
            .Select(x => new InstructorCardDto(
                x.Id,
                x.FirstName + " " + x.LastName,
                x.Specialization,
                x.PhotoUrl,
                x.PriceAzn
            ))
            .ToListAsync(ct);

        var response = new GetHomeQueryResponse(
            GreetingName: string.IsNullOrWhiteSpace(currentUser.FullName) ? "İstifadəçi" : currentUser.FullName,
            PopularAcademies: academies,
            Instructors: instructors
        );

        return Result<GetHomeQueryResponse>.Ok(response);
    }
}