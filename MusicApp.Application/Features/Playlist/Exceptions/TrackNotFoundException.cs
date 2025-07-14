namespace MusicApp.Application.Features.Playlist.Exceptions;

/// <summary>
/// Thrown when the track with given ID does not exist.
/// </summary>
public class TrackNotFoundException : Exception
{
    public TrackNotFoundException(Guid trackId)
        : base($"Track with ID '{trackId}' was not found.") { }
}
