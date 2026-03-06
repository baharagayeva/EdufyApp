using Edufy.Application.CQRS.Commands.Responses;
using Edufy.Domain.Commons;
using MediatR;

namespace Edufy.Application.CQRS.Commands.Requests;

public sealed record SaveLessonCommandRequest(int LessonId)
    : IRequest<Result<SaveLessonCommandResponse>>;