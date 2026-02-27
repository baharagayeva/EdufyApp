using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edufy.Application.CQRS.Queries.Requests;
using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Commons;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.QueryHandlers;

public sealed class GetCoursesQueryHandler(EdufyDbContext db)
    : IRequestHandler<GetCoursesQueryRequest, Result<IReadOnlyList<GetCoursesQueryResponse>>>
{
    public async Task<Result<IReadOnlyList<GetCoursesQueryResponse>>> Handle(
        GetCoursesQueryRequest request,
        CancellationToken ct)
    {
        if (request.Page <= 0)
            return Result<IReadOnlyList<GetCoursesQueryResponse>>.BadRequest("Page must be greater than 0.");

        if (request.Take <= 0 || request.Take > 100)
            return Result<IReadOnlyList<GetCoursesQueryResponse>>.BadRequest("Take must be between 1 and 100.");

        var query = db.Courses
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim();
            query = query.Where(x => EF.Functions.ILike(x.Title, $"%{s}%"));
        }

        var skip = (request.Page - 1) * request.Take;

        var items = await query
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

        return Result<IReadOnlyList<GetCoursesQueryResponse>>.Ok(items);
    }
}