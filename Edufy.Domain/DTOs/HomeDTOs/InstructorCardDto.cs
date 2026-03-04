namespace Edufy.Domain.DTOs.HomeDTOs;

public record InstructorCardDto(
    int Id,
    string FullName,
    string Specialization,
    string? PhotoUrl,
    decimal PriceAzn
);