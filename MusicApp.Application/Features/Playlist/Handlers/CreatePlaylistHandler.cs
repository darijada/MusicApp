using AutoMapper;
using MediatR;
using MusicApp.Application.Features.Playlist.Commands;
using MusicApp.Application.Features.Playlist.Repositories;
using PlaylistEntity = MusicApp.Core.Entities.Playlist.Playlist;

namespace MusicApp.Application.Features.Playlist.Handlers;

public class CreatePlaylistHandler : IRequestHandler<CreatePlaylistCommand, (Guid PlaylistId, string PlaylistName)>
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IMapper _mapper;

    public CreatePlaylistHandler(
        IPlaylistRepository playlistRepository,
        IMapper mapper)
    {
        _playlistRepository = playlistRepository;
        _mapper = mapper;
    }

    public async Task<(Guid PlaylistId, string PlaylistName)> Handle(CreatePlaylistCommand request, CancellationToken ct)
    {
        PlaylistEntity playlist = _mapper.Map<PlaylistEntity>(request.Dto);

        playlist.OwnerId = request.CurrentUserId;
        playlist.CreatedAt = DateTime.UtcNow;

        await _playlistRepository.CreateAsync(playlist, ct);
        return (playlist.Id, playlist.Name);
    }
}
