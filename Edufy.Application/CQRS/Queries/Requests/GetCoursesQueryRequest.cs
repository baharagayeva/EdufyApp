using Edufy.Application.CQRS.Queries.Responses;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public record GetCoursesQueryRequest(string? Search, int Page = 1, int Take = 20) : IRequest<IReadOnlyList<GetCoursesQueryResponse>>;