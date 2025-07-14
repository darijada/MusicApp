using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicApp.Core.Entities.Auth;
using MusicApp.Core.Entities.Playlist;
using MusicApp.Infrastructure.Persistence.Configurations.Playlist;
using MusicApp.Infrastructure.Persistence.Configurations.Auth;


namespace MusicApp.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Auth
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Playlist
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<PlaylistShare> PlaylistShares { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Auth
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());

            // Playlist
            builder.ApplyConfiguration(new PlaylistConfiguration());
            builder.ApplyConfiguration(new PlaylistTrackConfiguration());
            builder.ApplyConfiguration(new TrackConfiguration());
            builder.ApplyConfiguration(new PlaylistShareConfiguration());
        }
    }
}
