namespace MusicApp.Application.Features.Auth.Exceptions;

/// <summary>
/// Thrown when a provided refresh token is invalid or cannot be revoked.
/// </summary>
public class InvalidRefreshTokenException : Exception
{
    public InvalidRefreshTokenException()
        : base("The supplied refresh token is invalid or already revoked.")
    {
    }
}
