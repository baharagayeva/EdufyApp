using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Commands.Responses;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Entities;
using Edufy.Domain.Enums;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.CommandHandlers;

public sealed class ApplyToCourseCommandHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<ApplyToCourseCommandRequest, ApplyToCourseCommandResponse>
{
    public async Task<ApplyToCourseCommandResponse> Handle(ApplyToCourseCommandRequest request, CancellationToken ct)
    {
        // Variant B: Role seçimi var → Student müraciət edə bilər, Teacher yox.
        if (!string.Equals(currentUser.Role, UserRole.Student.ToString(), StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Yalnız tələbə müraciət edə bilər.");

        var courseExists = await db.Courses.AnyAsync(x => x.Id == request.CourseId && x.IsActive, ct);
        if (!courseExists) throw new KeyNotFoundException("Kurs tapılmadı");

        var already = await db.CourseApplications
            .AnyAsync(x => x.CourseId == request.CourseId && x.StudentUserId == currentUser.UserId, ct);

        if (already) throw new InvalidOperationException("Siz artıq bu kursa müraciət etmisiniz.");

        var app = new CourseApplication
        {
            CourseId = request.CourseId,
            StudentUserId = currentUser.UserId,
            Status = ApplicationStatus.Pending
        };

        db.CourseApplications.Add(app);
        await db.SaveChangesAsync(ct);

        return new ApplyToCourseCommandResponse(app.Id);
    }
}