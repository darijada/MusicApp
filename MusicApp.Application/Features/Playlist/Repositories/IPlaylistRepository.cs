using MusicApp.Core.Entities.Playlist;
using PlaylistEntity = MusicApp.Core.Entities.Playlist.Playlist;

namespace MusicApp.Application.Features.Playlist.Repositories
{
    public interface IPlaylistRepository
    {
        Task CreateAsync(PlaylistEntity playlist, CancellationToken cancellationToken);
        Task<PlaylistEntity> SearchByIdAsync(Guid playlistId, Guid userId, CancellationToken cancellationToken);
        Task<List<PlaylistEntity>> SearchByOwnerIdAsync(Guid userId, CancellationToken cancellationToken);
        Task AddTrackToPlaylistAsync(Guid playlistId, Track track, Guid userId, CancellationToken cancellationToken);
        Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId, Guid userId, CancellationToken cancellationToken);
        Task DeactivateAsync(Guid playlistId, Guid userId, CancellationToken cancellationToken);
    }
}
