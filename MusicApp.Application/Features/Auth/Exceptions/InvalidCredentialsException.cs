namespace MusicApp.Application.Features.Auth.Exceptions;

/// <summary>
/// Thrown when the provided username or password are invalid.
/// </summary>
public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
        : base("Invalid username or password.") { }
}
