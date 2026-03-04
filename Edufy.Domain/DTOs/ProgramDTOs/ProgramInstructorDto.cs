namespace Edufy.Domain.DTOs.ProgramDTOs;

public record ProgramInstructorDto(
    int Id,
    string FullName,
    string Specialization,
    string? Bio,
    string? PhotoUrl,
    string? LinkedInUrl
);