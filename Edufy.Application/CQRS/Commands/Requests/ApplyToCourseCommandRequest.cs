using Edufy.Application.CQRS.Commands.Responses;
using MediatR;

namespace Edufy.Application.CQRS.Commands.Requests;

public record ApplyToCourseCommandRequest(int CourseId) : IRequest<ApplyToCourseCommandResponse>; // returns ApplicationId