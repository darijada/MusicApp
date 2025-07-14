namespace MusicApp.Application.Features.Auth.Exceptions;

/// <summary>
/// Thrown when the confirmation code in the query string is malformed.
/// </summary>
public class InvalidConfirmationCodeException : Exception
{
    public InvalidConfirmationCodeException()
        : base("The email confirmation code is invalid or malformed.") { }
}
