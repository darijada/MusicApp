using MusicApp.Core.Entities.Playlist;

namespace MusicApp.Application.Features.Playlist.Repositories;

public interface IPlaylistTrackRepository
{
    Task AddAsync(PlaylistTrack playlistTrack, CancellationToken cancellationToken);
    Task<bool> IsDuplicateAsync(Guid playlistId, Guid TrackId, CancellationToken cancellationToken);
    Task DeactivateAsync(Guid playlistId, Guid trackId, CancellationToken cancellationToken);
}
