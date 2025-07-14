namespace MusicApp.Core.Entities.Playlist;

public class Track
{
    public Guid Id { get; set; }
    public string SpotifyTrackId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string AlbumName { get; set; } = null!;
    public string Artists { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    // navigation collection property
    public List<PlaylistTrack> PlaylistTracks { get; set; } = new();
}
