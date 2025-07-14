using AutoMapper;
using MediatR;
using MusicApp.Application.Features.Playlist.Dtos;
using MusicApp.Application.Features.Playlist.Queries;
using MusicApp.Application.Features.Playlist.Repositories;

public class GetUserPlaylistsHandler : IRequestHandler<GetUserPlaylistsQuery, List<PlaylistDto>>
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IMapper _mapper;

    public GetUserPlaylistsHandler(
        IPlaylistRepository playlistRepository, 
        IMapper mapper)
    {
        _playlistRepository = playlistRepository;
        _mapper = mapper;
    }

    public async Task<List<PlaylistDto>> Handle(GetUserPlaylistsQuery request, CancellationToken cancellationToken)
    {
        var playlists = await _playlistRepository.SearchByOwnerIdAsync(request.UserId, cancellationToken);

        return _mapper.Map<List<PlaylistDto>>(playlists);
    }
}
