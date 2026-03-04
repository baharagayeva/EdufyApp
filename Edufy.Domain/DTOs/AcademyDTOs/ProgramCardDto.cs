using Edufy.Domain.Enums;

namespace Edufy.Domain.DTOs.AcademyDTOs;

public record ProgramCardDto(
    int Id,
    string Name,
    string Duration,
    ProgramStatus Status,
    string? InstructorPhotoUrl
);