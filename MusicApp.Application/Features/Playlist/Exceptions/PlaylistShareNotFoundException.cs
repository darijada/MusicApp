namespace MusicApp.Application.Features.Playlist.Exceptions;

/// <summary>
/// Thrown when the playlist is not shared with the user
/// </summary>
public class PlaylistShareNotFoundException : Exception
{
    public PlaylistShareNotFoundException(Guid playlistId, Guid userId)
        : base($"Playlist with ID '{playlistId}' was not shared with user ID '{userId}'.") { }
}
