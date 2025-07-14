namespace MusicApp.Application.Features.Auth.Exceptions;

/// <summary>
/// Thrown when updating a user fails in the identity store
/// </summary>
public class UserUpdateFailedException : Exception
{
    public UserUpdateFailedException()
        : base("Failed to update the user.") { }

    public UserUpdateFailedException(string message)
        : base(message) { }

}
