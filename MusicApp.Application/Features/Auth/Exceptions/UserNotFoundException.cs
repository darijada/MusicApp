namespace MusicApp.Application.Features.Auth.Exceptions;

/// <summary>
/// Thrown when the user with given ID does not exist.
/// </summary>
public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid userId)
        : base($"User with ID '{userId}' was not found.") { }
}