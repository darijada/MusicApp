using MediatR;
using MusicApp.Application.Features.Playlist.Dtos;

namespace MusicApp.Application.Features.Playlist.Commands;

/// <summary>
/// Command to create a playlist
/// </summary>
public record CreatePlaylistCommand(CreatePlaylistDto Dto, Guid CurrentUserId) : IRequest<(Guid PlaylistId, string PlaylistName)>;
