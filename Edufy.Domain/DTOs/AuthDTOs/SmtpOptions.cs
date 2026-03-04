namespace Edufy.Domain.DTOs.AuthDTOs;

public sealed class SmtpOptions
{
    public string Host { get; set; } = default!;
    public int Port { get; set; }
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string FromEmail { get; set; } = default!;
    public string FromName { get; set; } = "Edufy";
    public bool UseSsl { get; set; }
}