namespace Edufy.Domain.DTOs.SavedProgramDTOs;

public record SavedProgramDto(
    int SavedProgramId,
    int ProgramId,
    string ProgramName,
    string AcademyName,
    string? AcademyLogoUrl,
    string Duration,
    string GroupSize,
    DateTime SavedAt
);