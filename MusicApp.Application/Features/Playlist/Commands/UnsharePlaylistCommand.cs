using MediatR;

namespace MusicApp.Application.Features.Playlist.Commands;

/// <summary>
/// Command to unshare a playlist with another user
/// </summary>
public record UnsharePlaylistCommand(Guid PlaylistId, Guid TargetUserId, Guid OwnerUserId) : IRequest<Unit>;
