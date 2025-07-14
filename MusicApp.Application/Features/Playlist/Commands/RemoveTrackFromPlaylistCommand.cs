using MediatR;

namespace MusicApp.Application.Features.Playlist.Commands;

/// <summary>
/// Command to remove track from a playlist
/// </summary>
public record RemoveTrackFromPlaylistCommand(Guid PlaylistId, Guid TrackId, Guid UserId) : IRequest<Unit>;