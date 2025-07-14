using MusicApp.Core.Entities.Playlist;

namespace MusicApp.Application.Features.Playlist.Repositories;

public interface ITrackRepository
{
    Task AddAsync(Track track, CancellationToken cancellationToken);
    Task<Track?> GetBySpotifyTrackIdAsync(string spotifyTrackId, CancellationToken cancellationToken);
    Task<List<Track>> GetAllTracksAsync(CancellationToken cancellationToken);
    Task DeactivateAsync(Guid trackId, CancellationToken cancellationToken);
}
