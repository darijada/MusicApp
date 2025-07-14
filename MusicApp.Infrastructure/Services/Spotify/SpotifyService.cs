using Microsoft.Extensions.Options;
using MusicApp.Application.Features.Spotify.Dtos;
using MusicApp.Application.Features.Spotify.Services;
using MusicApp.Infrastructure.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MusicApp.Infrastructure.Services.Spotify;

public class SpotifyService : ISpotifyService
{
    private readonly HttpClient _http;
    private readonly SpotifySettings _spotifySettings;
    private string? _accessToken;
    private DateTime _tokenExpiresAt;

    public SpotifyService(HttpClient http, IOptions<SpotifySettings> spotifySettings)
    {
        _http = http;
        _spotifySettings = spotifySettings.Value;
    }

    private async Task EnsureAppTokenAsync()
    {
        if (!string.IsNullOrEmpty(_accessToken) && _tokenExpiresAt > DateTime.UtcNow.AddMinutes(1))
            return;

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            })
        };

        string credentials = $"{_spotifySettings.ClientId}:{_spotifySettings.ClientSecret}";
        string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encoded);

        HttpResponseMessage response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        JsonElement json = await response.Content.ReadFromJsonAsync<JsonElement>();
        _accessToken = json.GetProperty("access_token").GetString();
        var expiresIn = json.GetProperty("expires_in").GetInt32();
        _tokenExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn);

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
    }

    public async Task<IEnumerable<SpotifyTrackDto>> SearchTracksAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Query cannot be empty.", nameof(query));

        await EnsureAppTokenAsync();

        string url = $"search?q={Uri.EscapeDataString(query)}&type=track&limit=20";
        JsonElement response = await _http.GetFromJsonAsync<JsonElement>(url);

        if (response.ValueKind != JsonValueKind.Object ||
            !response.TryGetProperty("tracks", out var tracksObj) ||
            !tracksObj.TryGetProperty("items", out var items))
        {
            throw new InvalidOperationException("Invalid Spotify API response.");
        }

        return items.EnumerateArray().Select(item => new SpotifyTrackDto
        {
            Id = item.GetProperty("id").GetString()!,
            Name = item.GetProperty("name").GetString()!,
            AlbumName = item.GetProperty("album").GetProperty("name").GetString(),
            Artists = item.GetProperty("artists")
                .EnumerateArray()
                .Select(a => a.GetProperty("name").GetString()!)
                .ToList()
        }).ToList();
    }

    public async Task<SpotifyTrackDto> GetTrackByIdAsync(string trackId)
    {
        if (string.IsNullOrWhiteSpace(trackId))
            throw new ArgumentException("Track ID cannot be empty.", nameof(trackId));

        await EnsureAppTokenAsync();

        JsonElement response = await _http.GetFromJsonAsync<JsonElement>($"tracks/{trackId}");

        if (response.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("Spotify returned invalid track data.");

        return new SpotifyTrackDto
        {
            Id = response.GetProperty("id").GetString()!,
            Name = response.GetProperty("name").GetString()!,
            AlbumName = response.GetProperty("album").GetProperty("name").GetString(),
            Artists = response.GetProperty("artists")
                .EnumerateArray()
                .Select(a => a.GetProperty("name").GetString()!)
                .ToList()
        };
    }
}
