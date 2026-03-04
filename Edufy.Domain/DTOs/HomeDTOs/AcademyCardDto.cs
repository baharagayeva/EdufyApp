namespace Edufy.Domain.DTOs.HomeDTOs;

public record AcademyCardDto(
    int Id,
    string Name,
    string? LogoUrl,
    int ProgramCount
);