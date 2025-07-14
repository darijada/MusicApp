namespace MusicApp.Application.Features.Playlist.Exceptions;

/// <summary>
/// Thrown when track is already added to the playlist
/// </summary>
public class DuplicatePlaylistTrackException : Exception
{
    public DuplicatePlaylistTrackException(Guid playlistId, Guid trackId)
        : base($"Track with ID '{trackId}' is already added to the playlist with ID '{playlistId}'.")
    {
    }
}
