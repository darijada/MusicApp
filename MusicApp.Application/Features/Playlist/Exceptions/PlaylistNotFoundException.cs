namespace MusicApp.Application.Features.Playlist.Exceptions;

/// <summary>
/// Thrown when the playlist with given ID does not exist.
/// </summary>
public class PlaylistNotFoundException : Exception
{
    public PlaylistNotFoundException(Guid playlistId)
        : base($"Playlist with ID '{playlistId}' was not found.") { }
}
