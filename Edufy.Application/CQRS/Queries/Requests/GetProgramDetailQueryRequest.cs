using Edufy.Application.CQRS.Queries.Responses;
using Edufy.Domain.Commons;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public record GetProgramDetailQueryRequest(int ProgramId)
    : IRequest<Result<GetProgramDetailQueryResponse>>;