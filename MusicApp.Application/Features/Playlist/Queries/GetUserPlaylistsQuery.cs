using MediatR;
using MusicApp.Application.Features.Playlist.Dtos;

namespace MusicApp.Application.Features.Playlist.Queries;

/// <summary>
/// Query to get all user playlists
/// </summary>
public record GetUserPlaylistsQuery(Guid UserId) : IRequest<List<PlaylistDto>>;
