using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Abstractions;
using Edufy.Domain.DTOs.HomeDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetHomeQueryHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<GetHomeQueryRequest, GetHomeQueryResponse>
{
    public async Task<GetHomeQueryResponse> Handle(GetHomeQueryRequest request, CancellationToken ct)
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

        return new GetHomeQueryResponse(
            GreetingName: currentUser.FullName ?? "İstifadəçi",
            PopularCourses: popularCourses,
            Instructors: instructors
        );
    }
}