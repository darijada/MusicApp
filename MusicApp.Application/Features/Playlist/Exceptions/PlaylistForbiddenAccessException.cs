namespace MusicApp.Application.Features.Playlist.Exceptions;

/// <summary>
/// Thrown when user doesn't have access to playlist
/// </summary>
public class PlaylistForbiddenAccessException : Exception
{
    public PlaylistForbiddenAccessException(Guid playlistId, Guid userId)
        : base($"Forbidden access to playlist with ID '{playlistId}' for user with ID '{userId}'.") { }
}
