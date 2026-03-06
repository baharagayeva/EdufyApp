using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.VideoCardDTOs;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetFeedVideosQueryHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<GetFeedVideosQueryRequest, Result<List<FeedVideoCardDto>>>
{
    public async Task<Result<List<FeedVideoCardDto>>> Handle(GetFeedVideosQueryRequest request, CancellationToken ct)
    {
        var userId = currentUser.UserId;

        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 50);
        var skip = (page - 1) * pageSize;

        var search = request.Search?.Trim();
        var like = !string.IsNullOrWhiteSpace(search) ? $"%{search}%" : null;

        var query = db.Lessons
            .AsNoTracking()
            .Where(l => l.IsDemo && l.VideoUrl != null && l.VideoUrl != "");

        if (like is not null && search!.Length >= 2)
        {
            query = query.Where(l =>
                EF.Functions.ILike(l.Name, like) ||
                EF.Functions.ILike(l.Module.Program.Name, like) ||
                EF.Functions.ILike(l.Module.Program.Instructor.FirstName, like) ||
                EF.Functions.ILike(l.Module.Program.Instructor.LastName, like));
        }

        var items = await query
            .OrderByDescending(l => l.Id)
            .Skip(skip)
            .Take(pageSize)
            .Select(l => new FeedVideoCardDto(
                l.Id,
                l.Name,
                l.ThumbnailUrl,
                l.DurationMinutes,
                l.VideoUrl!,
                l.Module.Program.InstructorId,
                l.Module.Program.Instructor.FirstName + " " + l.Module.Program.Instructor.LastName,
                l.Module.ProgramId,
                l.Module.Program.Name,
                userId != Guid.Empty &&
                         db.SavedLessons.Any(sl => sl.UserId == userId && sl.LessonId == l.Id)
            ))
            .ToListAsync(ct);

        return Result<List<FeedVideoCardDto>>.Ok(items);
    }
}