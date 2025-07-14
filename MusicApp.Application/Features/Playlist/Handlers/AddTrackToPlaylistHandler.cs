using AutoMapper;
using MediatR;
using MusicApp.Application.Features.Playlist.Commands;
using MusicApp.Application.Features.Playlist.Repositories;
using MusicApp.Core.Entities.Playlist;

namespace MusicApp.Application.Features.Playlist.Handlers;


public class AddTrackToPlaylistHandler : IRequestHandler<AddTrackToPlaylistCommand, Guid>
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IMapper _mapper;

    public AddTrackToPlaylistHandler(
        IPlaylistRepository playlistRepository,
        IMapper mapper)
    {
        _playlistRepository = playlistRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(AddTrackToPlaylistCommand request, CancellationToken cancellationToken)
    {
        Track track = _mapper.Map<Track>(request.Dto);
        await _playlistRepository.AddTrackToPlaylistAsync(request.PlaylistId, track, request.UserId, cancellationToken);
        return track.Id;
    }
}
