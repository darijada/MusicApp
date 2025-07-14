using MediatR;
using MusicApp.Application.Features.Spotify.Dtos;

namespace MusicApp.Application.Features.Spotify.Commands;

public record GetTrackByIdQuery(string TrackId) : IRequest<SpotifyTrackDto>;
