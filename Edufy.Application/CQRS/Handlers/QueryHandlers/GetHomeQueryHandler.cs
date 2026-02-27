using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        var popularCourses = await db.Courses
            .AsNoTracking()
            .Where(x => x.IsPopular && x.IsActive)
            .OrderByDescending(x => x.Id)
            .Take(request.PopularTake)
            .Select(x => new CourseCardDto(
                x.Id,
                x.Title,
                $"{x.DurationMonths} ay",
                x.IsActive,
                x.CoverImageUrl
            ))
            .ToListAsync(ct);

        var instructors = await db.InstructorProfiles
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Take(request.InstructorTake)
            .Select(x => new InstructorCardDto(
                x.Id,
                x.User.UserName ?? string.Empty,
                x.Title,
                x.PhotoUrl
            ))
            .ToListAsync(ct);

        var response = new GetHomeQueryResponse(
            GreetingName: string.IsNullOrWhiteSpace(currentUser.FullName) ? "İstifadəçi" : currentUser.FullName,
            PopularCourses: popularCourses,
            Instructors: instructors
        );

        return Result<GetHomeQueryResponse>.Ok(response);
    }
}