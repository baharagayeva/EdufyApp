namespace Edufy.Domain.DTOs.InstructorDTOs;

public sealed record InstructorProfileDto(
    int Id,
    string FullName,
    string Specialization,
    string? Bio,
    string? PhotoUrl,
    decimal PriceAzn,
    string? Address,
    string? PhoneNumber,
    string? LinkedInUrl,
    List<DemoVideoCardDto> DemoVideos
);