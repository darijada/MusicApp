namespace MusicApp.Application.Features.Auth.Exceptions;

/// <summary>
/// Thrown when ASP.NET Identity fails to confirm the email.
/// </summary>
public class EmailConfirmationFailedException : Exception
{
    public EmailConfirmationFailedException()
        : base("Email confirmation failed.") { }
}
