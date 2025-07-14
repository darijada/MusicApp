namespace MusicApp.Infrastructure.Configuration;

/// <summary>
/// Binds to the JwtSettings section in appsettings.json.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// The secret key used to sign the JWT.
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Expiration time in minutes.
    /// </summary>
    public int ExpiryMinutes { get; set; }
}
