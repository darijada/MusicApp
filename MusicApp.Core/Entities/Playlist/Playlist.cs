using MusicApp.Core.Entities.Auth;

namespace MusicApp.Core.Entities.Playlist;

public class Playlist
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    // navigation collection properties
    public User Owner { get; set; } = null!;
    public List<PlaylistTrack> PlaylistTracks { get; set; } = new();
}
