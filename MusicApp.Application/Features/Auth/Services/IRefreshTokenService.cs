using MusicApp.Core.Entities.Auth;

namespace MusicApp.Application.Features.Auth.Services;

public interface IRefreshTokenService
{
    Task RevokeAllTokensForUserAsync(Guid userId, CancellationToken ct);
    Task RevokeTokenAsync(string refreshToken, CancellationToken ct);
    Task<RefreshToken?> GetValidRefreshTokenAsync(string refreshToken, CancellationToken ct);
    Task CreateRefreshTokenAsync(RefreshToken token, CancellationToken ct);

}

