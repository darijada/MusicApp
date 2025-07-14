namespace MusicApp.Application.Features.Email.Services;

public interface IEmailSenderService
{
    Task SendEmailAsync(string to, string subject, string htmlMessage);
}
