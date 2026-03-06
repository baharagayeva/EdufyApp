using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.InstructorDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetInstructorProfileQueryHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<GetInstructorProfileQueryRequest, Result<InstructorProfileDto>>
{
    public async Task<Result<InstructorProfileDto>> Handle(GetInstructorProfileQueryRequest request, CancellationToken ct)
    {
        var userId = currentUser.UserId; // səndə necə adlanırsa uyğunlaşdır (Guid?)

        var instructor = await db.Instructors
            .AsNoTracking()
            .Where(i => i.Id == request.InstructorId)
            .Select(i => new
            {
                i.Id,
                FullName = i.FirstName + " " + i.LastName,
                i.Specialization,
                i.Bio,
                i.PhotoUrl,
                i.PriceAzn,
                i.Address,
                i.PhoneNumber,
                i.LinkedInUrl
            })
            .FirstOrDefaultAsync(ct);

        if (instructor is null)
            return Result<InstructorProfileDto>.NotFound("Instructor not found");

        // Demo videolar: Lesson -> Module -> Program -> InstructorId
        // Like status: SavedLessons
        var demoVideos = await db.Lessons
            .AsNoTracking()
            .Where(l =>
                l.IsDemo &&
                l.VideoUrl != null && l.VideoUrl != "" &&
                l.Module.Program.InstructorId == request.InstructorId)
            .OrderByDescending(l => l.Id)
            .Take(request.DemoTake)
            .Select(l => new DemoVideoCardDto(
                l.Id,
                l.Name,
                l.ThumbnailUrl,
                l.DurationMinutes,
                l.VideoUrl!,
                l.Module.Program.Name,
                l.Module.ProgramId,
                userId != Guid.Empty &&
                         db.SavedLessons.Any(sl => sl.UserId == userId && sl.LessonId == l.Id)
            ))
            .ToListAsync(ct);

        var response = new InstructorProfileDto(
            Id: instructor.Id,
            FullName: instructor.FullName,
            Specialization: instructor.Specialization,
            Bio: instructor.Bio,
            PhotoUrl: instructor.PhotoUrl,
            PriceAzn: instructor.PriceAzn,
            Address: instructor.Address,
            PhoneNumber: instructor.PhoneNumber,
            LinkedInUrl: instructor.LinkedInUrl,
            DemoVideos: demoVideos
        );

        return Result<InstructorProfileDto>.Ok(response);
    }
}