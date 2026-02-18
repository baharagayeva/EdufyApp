using Edufy.Application.CQRS.Queries.Responses;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public record GetHomeQueryRequest(int PopularTake = 6, int InstructorTake = 6) : IRequest<GetHomeQueryResponse>;