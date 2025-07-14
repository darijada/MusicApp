namespace MusicApp.Application.Features.Playlist.Exceptions;

/// <summary>
/// Thrown when playlist is already shared with the user
/// </summary>
public class PlaylistAlreadySharedException : Exception
{
    public PlaylistAlreadySharedException(Guid playlistId, Guid userId)
        : base($"Playlist with ID '{playlistId}' is already shared with user ID '{userId}'.")
    {
    }
}
