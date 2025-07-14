using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using MusicApp.Application.Features.Email.Services;
using MusicApp.Infrastructure.Configuration;

namespace MusicApp.Infrastructure.Services.Email;

/// <summary>
/// Sends email via SMTP using configured SmtpSettings.
/// </summary>
public class EmailSenderService : IEmailSenderService
{
    private readonly SmtpSettings _smtp;

    public EmailSenderService(IOptions<SmtpSettings> options)
    {
        _smtp = options.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        using var message = new MailMessage(
            new MailAddress(_smtp.From, _smtp.DisplayName),
            new MailAddress(to))
        {
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        using var client = new SmtpClient(_smtp.Host, _smtp.Port)
        {
            EnableSsl = _smtp.EnableSsl,
            Credentials = new NetworkCredential(_smtp.UserName, _smtp.Password)
        };

        await client.SendMailAsync(message);
    }
}