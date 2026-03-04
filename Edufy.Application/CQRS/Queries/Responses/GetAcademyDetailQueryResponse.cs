using Edufy.Domain.DTOs.AcademyDTOs;

namespace Edufy.Application.CQRS.Queries.Responses;

public record GetAcademyDetailQueryResponse(
    int Id,
    string Name,
    string? LogoUrl,
    string? About,
    int TotalApplications,
    int TotalStudents,
    decimal GraduationRate,
    List<ProgramCardDto> Programs
);