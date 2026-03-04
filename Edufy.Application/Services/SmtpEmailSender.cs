using Edufy.Domain.DTOs.AuthDTOs;
using Edufy.Domain.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Edufy.Application.Services;

public sealed class SmtpEmailSender(IOptions<SmtpOptions> opt) : IEmailSender
{
    private readonly SmtpOptions _opt = opt.Value;

    public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken ct)
    {
        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(_opt.FromName, _opt.FromEmail));
        msg.To.Add(MailboxAddress.Parse(toEmail));
        msg.Subject = subject;

        msg.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

        using var client = new SmtpClient();

        var secure = _opt.UseSsl
            ? SecureSocketOptions.SslOnConnect
            : SecureSocketOptions.StartTlsWhenAvailable;

        await client.ConnectAsync(_opt.Host, _opt.Port, secure, ct);

        if (!string.IsNullOrWhiteSpace(_opt.Username))
            await client.AuthenticateAsync(_opt.Username, _opt.Password, ct);

        await client.SendAsync(msg, ct);
        await client.DisconnectAsync(true, ct);
    }
}