using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.SavedProgramDTOs;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public record GetMySavedProgramsQueryRequest
    : IRequest<Result<List<SavedProgramDto>>>;