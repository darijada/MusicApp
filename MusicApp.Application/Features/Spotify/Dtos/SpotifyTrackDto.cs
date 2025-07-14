namespace MusicApp.Application.Features.Spotify.Dtos;

public class SpotifyTrackDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public IList<string> Artists { get; set; } = new List<string>();
    public string? AlbumName { get; set; }
}
