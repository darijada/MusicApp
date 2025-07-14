namespace MusicApp.Application.Features.Playlist.Dtos;

public class TrackDto
{
    public string SpotifyTrackId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public IList<string> Artists { get; set; } = new List<string>();
    public string AlbumName { get; set; } = null!;
}
