using MediatR;
using MusicApp.Application.Features.Spotify.Commands;
using MusicApp.Application.Features.Spotify.Dtos;
using MusicApp.Application.Features.Spotify.Services;

public class SearchTracksHandler : IRequestHandler<SearchTracksQuery, IEnumerable<SpotifyTrackDto>>
{
    private readonly ISpotifyService _spotifyService;

    public SearchTracksHandler(ISpotifyService spotify) => _spotifyService = spotify;

    public async Task<IEnumerable<SpotifyTrackDto>> Handle(SearchTracksQuery request, CancellationToken ct)
    {
        return await _spotifyService.SearchTracksAsync(request.Query);
    }
}
