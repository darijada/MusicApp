using AutoMapper;
using MusicApp.Application.Features.Auth.Dtos;
using MusicApp.Core.Entities.Auth;

namespace MusicApp.Application.Features.Auth.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterDto, User>();
    }
}
