using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Commons;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public sealed record GetCourseDetailQueryRequest(int CourseId)
    : IRequest<Result<GetCourseDetailQueryResponse>>;