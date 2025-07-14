namespace MusicApp.Application.Features.Playlist.Repositories;

public interface IPlaylistShareRepository
{
    Task<string> CreateShareAsync(Guid playlistId, Guid ownerUserId, Guid targetUserId, CancellationToken cancellationToken);
    Task<string> DeactivateShareAsync(Guid playlistId, Guid ownerUserId, Guid targetUserId, CancellationToken cancellationToken);
    Task<List<Guid>> GetUserIdsSharedWithAsync(Guid playlistId, CancellationToken cancellationToken);

}
