using System.Threading.Tasks;
using MusicApp.Application.Features.Spotify.Dtos;

namespace MusicApp.Application.Features.Spotify.Services
{
    public interface ISpotifyService
    {
        Task<IEnumerable<SpotifyTrackDto>> SearchTracksAsync(string query);
        Task<SpotifyTrackDto> GetTrackByIdAsync(string trackId);

    }

}