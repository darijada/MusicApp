using AutoMapper;
using MusicApp.Application.Features.Playlist.Dtos;
using MusicApp.Core.Entities.Playlist;
using PlaylistEntity = MusicApp.Core.Entities.Playlist.Playlist;

namespace MusicApp.Application.Features.Playlist.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Playlist -> PlaylistDto
        CreateMap<PlaylistEntity, PlaylistDto>()
            .ForMember(dest => dest.Tracks, opt => opt.MapFrom(src =>
                src.PlaylistTracks
                    .Where(pt => pt.IsActive && pt.Track != null)
                    .Select(pt => pt.Track)
            ));

        // CreatePlaylistDto -> Playlist
        CreateMap<CreatePlaylistDto, PlaylistEntity>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
            .ForMember(dest => dest.PlaylistTracks, opt => opt.Ignore());

        // Track -> TrackDto
        CreateMap<Track, TrackDto>()
            .ForMember(dest => dest.Artists,
                opt => opt.MapFrom(src =>
                    src.Artists.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList()));

        // TrackDto -> Track
        CreateMap<TrackDto, Track>()
            .ForMember(dest => dest.Artists,
                opt => opt.MapFrom(src => string.Join(", ", src.Artists)))
            .ForMember(dest => dest.PlaylistTracks, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());


        // AddTrackToPlaylistDto -> Track
        CreateMap<AddTrackToPlaylistDto, Track>()
            .ForMember(dest => dest.Artists, opt => opt.MapFrom(src => string.Join(", ", src.Artists)))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.PlaylistTracks, opt => opt.Ignore());

        // PlaylistShareDto -> PlaylistShare
        CreateMap<PlaylistShareDto, PlaylistShare>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Playlist, opt => opt.Ignore());
    }
}
