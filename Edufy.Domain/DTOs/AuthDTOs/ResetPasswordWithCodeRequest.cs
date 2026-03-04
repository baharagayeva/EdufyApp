namespace Edufy.Domain.DTOs.AuthDTOs;

public sealed record ResetPasswordWithCodeRequest(string Email, string Code, string NewPassword);