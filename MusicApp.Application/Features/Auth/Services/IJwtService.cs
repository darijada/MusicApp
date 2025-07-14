using MusicApp.Core.Entities.Auth;

namespace MusicApp.Application.Features.Auth.Services;

public interface IJwtService
{
    string GenerateToken(Guid userId, string userName, string email);
    (string AccessToken, RefreshToken RefreshToken) GenerateTokens(User user);
}
