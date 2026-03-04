using Edufy.Application.CQRS.Commands.Responses;
using Edufy.Domain.Commons;
using MediatR;

namespace Edufy.Application.CQRS.Commands.Requests;

public record ApplyCommandRequest(
    int ProgramId,
    string FirstName,
    string LastName,
    string PhoneNumber
) : IRequest<Result<ApplyCommandResponse>>;