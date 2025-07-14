using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Features.Playlist.Exceptions;
using MusicApp.Application.Features.Playlist.Repositories;
using MusicApp.Core.Entities.Playlist;
using MusicApp.Infrastructure.Persistence;

public class TrackRepository : ITrackRepository
{
    private readonly AppDbContext _appDbContext;

    public TrackRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(Track track, CancellationToken cancellationToken)
    {
        _appDbContext.Tracks.Add(track);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Track?> GetBySpotifyTrackIdAsync(string spotifyTrackId, CancellationToken cancellationToken)
    {
        return await _appDbContext.Tracks.FirstOrDefaultAsync(t => t.SpotifyTrackId == spotifyTrackId, cancellationToken);
    }

    public async Task<List<Track>> GetAllTracksAsync(CancellationToken cancellationToken)
    {
        return await _appDbContext.Tracks
            .IgnoreQueryFilters()
            .ToListAsync(cancellationToken);
    }

    public async Task DeactivateAsync(Guid trackId, CancellationToken cancellationToken)
    {
        Track? track = await _appDbContext.Tracks.FirstOrDefaultAsync(t => t.Id == trackId, cancellationToken);

        if (track is null)
            throw new TrackNotFoundException(trackId);

        track.IsActive = false;
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
