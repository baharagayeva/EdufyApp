using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.InstructorDTOs;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public sealed record GetInstructorProfileQueryRequest(
    int InstructorId,
    int DemoTake
) : IRequest<Result<InstructorProfileDto>>;