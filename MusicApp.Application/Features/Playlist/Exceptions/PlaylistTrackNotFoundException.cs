namespace MusicApp.Application.Features.Playlist.Exceptions;

/// <summary>
/// Thrown when the track is not on the playlist.
/// </summary>
public class PlaylistTrackNotFoundException : Exception
{
    public PlaylistTrackNotFoundException(Guid playlistId, Guid trackId)
        : base($"Track with ID '{trackId}' was not found on playlist with ID '{playlistId}'.") { }
}
