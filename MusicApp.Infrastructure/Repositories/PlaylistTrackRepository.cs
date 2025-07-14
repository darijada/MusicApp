using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Features.Playlist.Exceptions;
using MusicApp.Application.Features.Playlist.Repositories;
using MusicApp.Core.Entities.Playlist;
using MusicApp.Infrastructure.Persistence;

namespace MusicApp.Infrastructure.Repositories;

public class PlaylistTrackRepository : IPlaylistTrackRepository
{
    private readonly AppDbContext _appDbContext;
    
    public PlaylistTrackRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(PlaylistTrack playlistTrack, CancellationToken cancellationToken)
    {
        await _appDbContext.PlaylistTracks.AddAsync(playlistTrack, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateAsync(Guid playlistId, Guid trackId, CancellationToken cancellationToken)
    {
        return await _appDbContext.PlaylistTracks
            .AnyAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId, cancellationToken);
    }

    public async Task DeactivateAsync(Guid playlistId, Guid trackId, CancellationToken cancellationToken)
    {
        PlaylistTrack? playlistTrack = await _appDbContext.PlaylistTracks
            .FirstOrDefaultAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId, cancellationToken);

        if (playlistTrack is null)
            throw new PlaylistTrackNotFoundException(playlistId, trackId);

        playlistTrack.IsActive = false;
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
