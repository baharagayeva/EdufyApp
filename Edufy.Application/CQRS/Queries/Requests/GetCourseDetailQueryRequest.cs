using Edufy.Application.CQRS.Queries.Responses;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public record GetCourseDetailQueryRequest(int CourseId) : IRequest<GetCourseDetailQueryResponse>;