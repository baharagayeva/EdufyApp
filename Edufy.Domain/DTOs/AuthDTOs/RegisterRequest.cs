namespace Edufy.Domain.DTOs.AuthDTOs;

public record RegisterRequest(string FullName, string Email, string Password);