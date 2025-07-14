using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Features.Auth.Services;
using MusicApp.Core.Entities.Auth;
using MusicApp.Infrastructure.Persistence;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly AppDbContext _dbContext;
    public RefreshTokenService(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task RevokeAllTokensForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.Expires > DateTime.UtcNow)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(rt => rt.IsRevoked, true),
                cancellationToken);
    }

    public async Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        RefreshToken? token = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

        if (token != null && !token.IsRevoked && token.Expires > DateTime.UtcNow)
        {
            token.Revoke();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }      
    }

    public async Task<RefreshToken?> GetValidRefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        return await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt =>
                rt.Token == refreshToken
                && !rt.IsRevoked
                && rt.Expires > DateTime.UtcNow, ct);
    }

    public async Task CreateRefreshTokenAsync(RefreshToken token, CancellationToken ct)
    {
        _dbContext.RefreshTokens.Add(token);
        await _dbContext.SaveChangesAsync(ct);
    }
}
