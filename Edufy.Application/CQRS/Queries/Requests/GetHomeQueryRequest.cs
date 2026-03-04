using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Commons;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public record GetHomeQueryRequest(
    int PopularTake = 5,
    int InstructorTake = 4
) : IRequest<Result<GetHomeQueryResponse>>;