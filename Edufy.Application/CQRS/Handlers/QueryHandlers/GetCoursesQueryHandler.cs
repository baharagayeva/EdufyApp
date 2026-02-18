using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Application.CQRS.Queries.Responses;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetCoursesQueryHandler(EdufyDbContext db)
    : IRequestHandler<GetCoursesQueryRequest, IReadOnlyList<GetCoursesQueryResponse>>
{
    public async Task<IReadOnlyList<GetCoursesQueryResponse>> Handle(GetCoursesQueryRequest request, CancellationToken ct)
    {
        var query = db.Courses.AsNoTracking().Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(x => EF.Functions.ILike(x.Title, $"%{s}%"));
        }

        var skip = (request.Page <= 1 ? 0 : (request.Page - 1) * request.Take);

        return await query
            .OrderByDescending(x => x.IsPopular)
            .ThenByDescending(x => x.Id)
            .Skip(skip)
            .Take(request.Take)
            .Select(x => new GetCoursesQueryResponse(
                x.Id,
                x.Title,
                $"{x.DurationMonths} ay",
                x.IsActive,
                x.CoverImageUrl
            ))
            .ToListAsync(ct);
    }
}