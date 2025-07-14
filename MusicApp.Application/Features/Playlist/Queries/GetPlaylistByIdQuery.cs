using global::MusicApp.Application.Features.Playlist.Dtos;
using MediatR;

namespace MusicApp.Application.Features.Playlist.Queries;

/// <summary>
/// Query to get a playlist by ID
/// </summary>
public record GetPlaylistByIdQuery(Guid PlaylistId, Guid UserId) : IRequest<PlaylistDto>;

