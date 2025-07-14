using MediatR;

namespace MusicApp.Application.Features.Playlist.Commands;

/// <summary>
/// Command to share a playlist with another user
/// </summary>
public record SharePlaylistCommand(Guid PlaylistId, Guid TargetUserId, Guid OwnerUserId) : IRequest<Unit>;
