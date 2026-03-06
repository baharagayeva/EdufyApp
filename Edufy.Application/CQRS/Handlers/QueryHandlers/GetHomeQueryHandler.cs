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
        var greetingName = string.IsNullOrWhiteSpace(currentUser.FullName)
            ? "İstifadəçi"
            : currentUser.FullName;

        var search = request.Search?.Trim();

        // Search yoxdursa -> mövcud Home logic
        if (string.IsNullOrWhiteSpace(search))
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

            var response0 = new GetHomeQueryResponse(
                GreetingName: greetingName,
                PopularAcademies: academies,
                Instructors: instructors
            );

            return Result<GetHomeQueryResponse>.Ok(response0);
        }

        // Search minimum uzunluq (UI üçün rahatdır)
        if (search.Length < 2)
        {
            var empty = new GetHomeQueryResponse(
                GreetingName: greetingName,
                PopularAcademies: [],
                Instructors: []
            );

            return Result<GetHomeQueryResponse>.Ok(empty);
        }

        var like = $"%{search}%";

        // Search -> Academies
        var academiesFiltered = await db.Academies
            .AsNoTracking()
            .Where(x => EF.Functions.ILike(x.Name, like))
            .OrderByDescending(x => x.TotalStudents)
            .Take(request.PopularTake)
            .Select(x => new AcademyCardDto(
                x.Id,
                x.Name,
                x.LogoUrl,
                x.Programs.Count
            ))
            .ToListAsync(ct);

        // Search -> Instructors
        var instructorsFiltered = await db.Instructors
            .AsNoTracking()
            .Where(x =>
                EF.Functions.ILike(x.FirstName, like) ||
                EF.Functions.ILike(x.LastName, like) ||
                EF.Functions.ILike((x.FirstName + " " + x.LastName), like) ||
                EF.Functions.ILike(x.Specialization ?? "", like)
            )
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
            GreetingName: greetingName,
            PopularAcademies: academiesFiltered,
            Instructors: instructorsFiltered
        );

        return Result<GetHomeQueryResponse>.Ok(response);
    }
}