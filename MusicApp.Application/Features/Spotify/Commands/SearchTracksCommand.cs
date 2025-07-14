using MediatR;
using MusicApp.Application.Features.Spotify.Dtos;

namespace MusicApp.Application.Features.Spotify.Commands;

public record SearchTracksQuery(string Query) : IRequest<IEnumerable<SpotifyTrackDto>>;
