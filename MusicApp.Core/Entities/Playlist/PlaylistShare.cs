using MusicApp.Core.Entities.Auth;

namespace MusicApp.Core.Entities.Playlist;

public class PlaylistShare
{
    public Guid PlaylistId { get; set; }
    public Guid UserId { get; set; }
    public bool IsActive { get; set; } = true;

    public Playlist? Playlist { get; set; }
    public User? User { get; set; }
}

