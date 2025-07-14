namespace MusicApp.Core.Entities.Auth;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; } = false;
    
    public User? User { get; set; }
    

    public bool IsExpired => DateTime.UtcNow >= Expires;
    
    public bool IsActive => !IsRevoked && !IsExpired;

    public void Revoke() => IsRevoked = true;
}
