namespace MusicApp.Application.Features.Auth.Exceptions;

/// <summary>
/// Thrown when the user’s email has not yet been confirmed.
/// </summary>
public class EmailNotConfirmedException : Exception
{
    public EmailNotConfirmedException()
        : base("Email not confirmed.") { }
}