using System;
using System.Threading;
using System.Threading.Tasks;
using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Commands.Responses;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.Entities;
using Edufy.Domain.Enums;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.CommandHandlers;

public sealed class ApplyToCourseCommandHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<ApplyToCourseCommandRequest, Result<ApplyToCourseCommandResponse>>
{
    public async Task<Result<ApplyToCourseCommandResponse>> Handle(ApplyToCourseCommandRequest request, CancellationToken ct)
    {
        // User identity yoxdursa
        if (currentUser.UserId == Guid.Empty)
            return Result<ApplyToCourseCommandResponse>.Unauthorized("User is not authenticated.");

        // Variant B: yalnız Student müraciət edə bilər
        if (!string.Equals(currentUser.Role, UserRole.Student.ToString(), StringComparison.OrdinalIgnoreCase))
            return Result<ApplyToCourseCommandResponse>.Forbidden("Only students can apply to courses.");

        var courseExists = await db.Courses
            .AnyAsync(x => x.Id == request.CourseId && x.IsActive, ct);

        if (!courseExists)
            return Result<ApplyToCourseCommandResponse>.NotFound("Course not found.");

        var alreadyApplied = await db.CourseApplications
            .AnyAsync(x => x.CourseId == request.CourseId && x.StudentUserId == currentUser.UserId, ct);

        if (alreadyApplied)
            return Result<ApplyToCourseCommandResponse>.Conflict("You have already applied to this course.");

        var application = new CourseApplication
        {
            CourseId = request.CourseId,
            StudentUserId = currentUser.UserId,
            Status = ApplicationStatus.Pending
        };

        db.CourseApplications.Add(application);
        await db.SaveChangesAsync(ct);

        var response = new ApplyToCourseCommandResponse(application.Id);

        return Result<ApplyToCourseCommandResponse>.Created(response, "Application submitted successfully.");
    }
}