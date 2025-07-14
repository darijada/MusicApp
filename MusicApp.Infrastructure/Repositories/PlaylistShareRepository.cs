using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Features.Playlist.Exceptions;
using MusicApp.Application.Features.Playlist.Repositories;
using MusicApp.Core.Entities.Playlist;
using MusicApp.Infrastructure.Persistence;

namespace MusicApp.Infrastructure.Repositories;

public class PlaylistShareRepository : IPlaylistShareRepository
{
    private readonly AppDbContext _context;
    private readonly IPlaylistRepository _playlistRepository;

    public PlaylistShareRepository(
        AppDbContext context,
        IPlaylistRepository playlistRepository)
    {
        _context = context;
        _playlistRepository = playlistRepository;
    }

    public async Task<string> CreateShareAsync(Guid playlistId, Guid ownerUserId, Guid targetUserId, CancellationToken cancellationToken)
    {
        Playlist? playlist = await _playlistRepository.SearchByIdAsync(playlistId, ownerUserId, cancellationToken);

        bool alreadyShared = await _context.PlaylistShares
            .AnyAsync(x => x.PlaylistId == playlistId && x.UserId == targetUserId && x.IsActive, cancellationToken);

        if (alreadyShared)
            throw new PlaylistAlreadySharedException(playlistId, targetUserId);

        PlaylistShare share = new PlaylistShare
        {
            PlaylistId = playlistId,
            UserId = targetUserId
        };

        _context.PlaylistShares.Add(share);
        await _context.SaveChangesAsync(cancellationToken);

        return playlist.Name;
    }

    public async Task<string> DeactivateShareAsync(Guid playlistId, Guid ownerUserId, Guid targetUserId, CancellationToken cancellationToken)
    {
        Playlist? playlist = await _playlistRepository.SearchByIdAsync(playlistId, ownerUserId, cancellationToken);

        PlaylistShare? share = await _context.PlaylistShares
            .FirstOrDefaultAsync(x => x.PlaylistId == playlistId && x.UserId == targetUserId && x.IsActive, cancellationToken);

        if (share is null)
            throw new PlaylistShareNotFoundException(playlistId, targetUserId);

        share.IsActive = false;

        _context.PlaylistShares.Update(share);
        await _context.SaveChangesAsync(cancellationToken);

        return playlist.Name;

    }
    public async Task<List<Guid>> GetUserIdsSharedWithAsync(Guid playlistId, CancellationToken cancellationToken)
    {
        return await _context.PlaylistShares
            .Where(x => x.PlaylistId == playlistId && x.IsActive)
            .Select(x => x.UserId)
            .ToListAsync(cancellationToken);
    }
}
