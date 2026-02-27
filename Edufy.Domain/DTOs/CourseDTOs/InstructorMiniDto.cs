namespace Edufy.Domain.DTOs.CourseDTOs;

public record InstructorMiniDto(
    int Id,
    string FullName,
    string Title,
    string Bio,
    string? PhotoUrl,
    string? LinkedInUrl
);