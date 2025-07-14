using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicApp.Core.Entities.Playlist;

namespace MusicApp.Infrastructure.Persistence.Configurations.Playlist;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> b)
    {
        b.HasKey(t => t.Id);

        b.Property(t => t.SpotifyTrackId)
            .IsRequired();

        b.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        b.Property(t => t.AlbumName)
            .IsRequired()
            .HasMaxLength(200);

        b.Property(t => t.Artists)
            .IsRequired()
            .HasMaxLength(500);

        b.Property(t => t.CreatedAt)
            .IsRequired();

        b.HasQueryFilter(t => t.IsActive);

        b.HasMany(t => t.PlaylistTracks)
            .WithOne(pt => pt.Track)
            .HasForeignKey(pt => pt.TrackId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
