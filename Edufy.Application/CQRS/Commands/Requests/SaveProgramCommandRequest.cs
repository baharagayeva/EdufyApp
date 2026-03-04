using Edufy.Application.CQRS.Commands.Responses;
using Edufy.Domain.Commons;
using MediatR;

namespace Edufy.Application.CQRS.Commands.Requests;

public record SaveProgramCommandRequest(int ProgramId)
    : IRequest<Result<SaveProgramCommandResponse>>;