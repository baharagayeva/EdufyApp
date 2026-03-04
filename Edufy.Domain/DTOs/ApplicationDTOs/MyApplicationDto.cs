using Edufy.Domain.Enums;

namespace Edufy.Domain.DTOs.ApplicationDTOs;

public record MyApplicationDto(
    int ApplicationId,
    int ProgramId,
    string ProgramName,
    string AcademyName,
    string? AcademyLogoUrl,
    string Duration,
    ApplicationStatus Status,
    DateTime AppliedAt
);