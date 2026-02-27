using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.CourseDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetCourseDetailQueryHandler(EdufyDbContext db)
    : IRequestHandler<GetCourseDetailQueryRequest, Result<GetCourseDetailQueryResponse>>
{
    public async Task<Result<GetCourseDetailQueryResponse>> Handle(GetCourseDetailQueryRequest request, CancellationToken ct)
    {
        var course = await db.Courses
            .AsNoTracking()
            .Where(x => x.Id == request.CourseId) // istəsən && x.IsActive əlavə et
            .Select(x => new
            {
                x.Id,
                x.Title,
                x.Description,
                x.DurationMonths,
                x.GroupSizeMin,
                x.GroupSizeMax,
                Modules = x.Modules
                    .OrderBy(m => m.Order)
                    .Select(m => new ModuleDto(m.Id, m.Title, m.Order))
                    .ToList(),
                Instructor = new InstructorMiniDto(
                    x.InstructorProfile.Id,
                    x.InstructorProfile.User.UserName ?? string.Empty,
                    x.InstructorProfile.Title,
                    x.InstructorProfile.Bio,
                    x.InstructorProfile.PhotoUrl,
                    x.InstructorProfile.LinkedInUrl
                )
            })
            .FirstOrDefaultAsync(ct);

        if (course is null)
            return Result<GetCourseDetailQueryResponse>.NotFound("Course not found.");

        var response = new GetCourseDetailQueryResponse(
            Id: course.Id,
            Title: course.Title,
            Description: course.Description,
            DurationMonths: course.DurationMonths,
            GroupSizeText: $"{course.GroupSizeMin}-{course.GroupSizeMax} nəfər",
            Modules: course.Modules,
            Instructor: course.Instructor
        );

        return Result<GetCourseDetailQueryResponse>.Ok(response);
    }
}