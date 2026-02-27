using System.Collections.Generic;
using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Commons;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public sealed record GetCoursesQueryRequest(string? Search, int Page = 1, int Take = 20)
    : IRequest<Result<IReadOnlyList<GetCoursesQueryResponse>>>;