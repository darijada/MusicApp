using Microsoft.AspNetCore.Identity;

namespace MusicApp.Core.Entities.Auth;

public class User : IdentityUser<Guid> 
{
    public bool IsActive { get; set; } = false;
}
