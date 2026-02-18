using Edufy.Domain.DTOs.CourseDTOs;

namespace Edufy.Application.CQRS.Queries.Responses;

public record GetCourseDetailQueryResponse(
    int Id,
    string Title,
    string Description,
    int DurationMonths,
    string GroupSizeText,
    IReadOnlyList<ModuleDto> Modules,
    InstructorMiniDto Instructor
);