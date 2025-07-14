using AutoMapper;
using MediatR;
using MusicApp.Application.Features.Playlist.Dtos;
using MusicApp.Application.Features.Playlist.Queries;
using MusicApp.Application.Features.Playlist.Repositories;
using PlaylistEntity = MusicApp.Core.Entities.Playlist.Playlist;

public class GetPlaylistByIdHandler : IRequestHandler<GetPlaylistByIdQuery, PlaylistDto>
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IMapper _mapper;

    public GetPlaylistByIdHandler(
        IPlaylistRepository playlistRepository,
        IMapper mapper)
    {
        _playlistRepository = playlistRepository;
        _mapper = mapper;
    }

    public async Task<PlaylistDto> Handle(GetPlaylistByIdQuery request, CancellationToken cancellationToken)
    {
        PlaylistEntity? playlist = await _playlistRepository.SearchByIdAsync(request.PlaylistId, request.UserId, cancellationToken);

        return _mapper.Map<PlaylistDto>(playlist);
    }
}
