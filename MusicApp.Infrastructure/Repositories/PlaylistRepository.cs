using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Features.Playlist.Exceptions;
using MusicApp.Application.Features.Playlist.Repositories;
using MusicApp.Core.Entities.Playlist;
using MusicApp.Infrastructure.Persistence;

namespace MusicApp.Infrastructure.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IPlaylistTrackRepository _playlistTrackRepository;
    private readonly ITrackRepository _trackRepository;

    public PlaylistRepository(
        AppDbContext appDbContext,
        IPlaylistTrackRepository playlistTrackRepository,
        ITrackRepository trackRepository)
    {
        _appDbContext = appDbContext;
        _playlistTrackRepository = playlistTrackRepository;
        _trackRepository = trackRepository;
    }

    public async Task CreateAsync(Playlist playlist, CancellationToken cancellationToken)
    {
        await _appDbContext.Playlists.AddAsync(playlist, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Playlist> SearchByIdAsync(Guid playlistId, Guid userId, CancellationToken cancellationToken)
    {
        Playlist? playlist = await _appDbContext.Playlists
            .Include(p => p.PlaylistTracks)
            .ThenInclude(pt => pt.Track)
            .FirstOrDefaultAsync(p => p.Id == playlistId, cancellationToken);

        if (playlist is null)
            throw new PlaylistNotFoundException(playlistId);
        
        if (playlist.OwnerId != userId)
            throw new PlaylistForbiddenAccessException(playlistId, userId);

        return playlist;
    }

    public async Task<List<Playlist>> SearchByOwnerIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _appDbContext.Playlists
            .Where(p => p.OwnerId == userId)
            .Include(p => p.PlaylistTracks)
            .ThenInclude(pt => pt.Track)
            .ToListAsync(cancellationToken);
    }

    public async Task AddTrackToPlaylistAsync(Guid playlistId, Track track, Guid userId, CancellationToken cancellationToken)
    {
        await SearchByIdAsync(playlistId, userId, cancellationToken);

        Track? existingTrack = await _trackRepository.GetBySpotifyTrackIdAsync(track.SpotifyTrackId, cancellationToken);

        if (existingTrack is null)
        {
            track.CreatedAt = DateTime.Now;
            await _trackRepository.AddAsync(track, cancellationToken);
        }

        bool isDuplicatePlaylistTrack = await _playlistTrackRepository.IsDuplicateAsync(playlistId, track.Id, cancellationToken);

        if (isDuplicatePlaylistTrack)
            throw new DuplicatePlaylistTrackException(playlistId, track.Id);

        PlaylistTrack playlistTrack = new PlaylistTrack
        {
            PlaylistId = playlistId,
            TrackId = track.Id,
            CreatedAt = DateTime.Now
        };

        await _playlistTrackRepository.AddAsync(playlistTrack, cancellationToken);
    }

    public async Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId, Guid userId, CancellationToken cancellationToken)
    {
        await SearchByIdAsync(playlistId, userId, cancellationToken);
        await _playlistTrackRepository.DeactivateAsync(playlistId, trackId, cancellationToken);
    }

    public async Task DeactivateAsync(Guid playlistId, Guid userId, CancellationToken cancellationToken)
    {
        Playlist? playlist = await SearchByIdAsync(playlistId, userId, cancellationToken);

        foreach (var pt in playlist.PlaylistTracks)
            pt.IsActive = false;

        playlist.IsActive = false;
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
