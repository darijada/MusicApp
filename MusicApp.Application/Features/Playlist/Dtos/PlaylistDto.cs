namespace MusicApp.Application.Features.Playlist.Dtos;

public class PlaylistDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; } = null!;
    public List<TrackDto> Tracks { get; set; } = new List<TrackDto>();
}
