using Edufy.Domain.DTOs.ProgramDTOs;

namespace Edufy.Application.CQRS.Queries.Responses;

public record GetProgramDetailQueryResponse(
    int Id,
    string Name,
    string? About,
    string Duration,
    string GroupSize,
    List<ModuleDto> Modules,
    ProgramInstructorDto Instructor
);