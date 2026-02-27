namespace Edufy.Domain.DTOs.HomeDTOs;

public sealed record InstructorCardDto(
    int Id,
    string FullName,
    string Title,
    string? PhotoUrl
);