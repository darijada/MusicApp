using MediatR;
using MusicApp.Application.Features.Spotify.Commands;
using MusicApp.Application.Features.Spotify.Dtos;
using MusicApp.Application.Features.Spotify.Services;

public class GetTrackByIdHandler : IRequestHandler<GetTrackByIdQuery, SpotifyTrackDto>
{
    private readonly ISpotifyService _spotify;

    public GetTrackByIdHandler(ISpotifyService spotify) => _spotify = spotify;

    public async Task<SpotifyTrackDto> Handle(GetTrackByIdQuery request, CancellationToken ct)
    {
        return await _spotify.GetTrackByIdAsync(request.TrackId);
    }
}
