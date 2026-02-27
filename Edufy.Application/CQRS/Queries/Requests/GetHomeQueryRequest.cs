using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Commons;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public sealed record GetHomeQueryRequest(
    int PopularTake = 6,
    int InstructorTake = 6
) : IRequest<Result<GetHomeQueryResponse>>;