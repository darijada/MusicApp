namespace MusicApp.Application.Features.Auth.Exceptions;

/// <summary>
/// Thrown when attempting to invoke a protected action with an invalid or missing token.
/// </summary>
public class UnauthorizedActionException : Exception
{
    public UnauthorizedActionException(string message = "Unauthorized.")
        : base(message) { }
}