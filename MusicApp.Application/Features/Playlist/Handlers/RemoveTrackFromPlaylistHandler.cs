using AutoMapper;
using MediatR;
using MusicApp.Application.Features.Playlist.Commands;
using MusicApp.Application.Features.Playlist.Repositories;

namespace MusicApp.Application.Features.Playlist.Handlers;


public class RemoveTrackFromPlaylistHandler : IRequestHandler<RemoveTrackFromPlaylistCommand, Unit>
{
    private readonly IPlaylistRepository _playlistRepository;

    public RemoveTrackFromPlaylistHandler(
        IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<Unit> Handle(RemoveTrackFromPlaylistCommand request, CancellationToken cancellationToken)
    {
        await _playlistRepository.RemoveTrackFromPlaylistAsync(request.PlaylistId, request.TrackId, request.UserId, cancellationToken);
        return Unit.Value; // MediatR-s void
    }
}
