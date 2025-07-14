using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Application.Features.Auth.Services;
using MusicApp.Core.Entities.Auth;
using MusicApp.Infrastructure.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MusicApp.Infrastructure.Services.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        public string GenerateToken(Guid userId, string userName, string email)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Email, email),
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string AccessToken, RefreshToken RefreshToken) GenerateTokens(User user)
        {
            var accessToken = GenerateToken(user.Id, user.UserName!, user.Email!);

            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            var refreshTokenString = WebEncoders.Base64UrlEncode(randomBytes);

            var refreshToken = new RefreshToken { 
                Token = refreshTokenString,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            return (accessToken, refreshToken);
        }
    }
}
