namespace MusicApp.Core.Entities.Playlist;

public class PlaylistTrack
{
    public Guid PlaylistId { get; set; }
    public Guid TrackId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    // navigation collection properties
    public Playlist? Playlist { get; set; } = null!;
    public Track? Track { get; set; } = null!; 
}
