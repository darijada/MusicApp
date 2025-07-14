using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicApp.Core.Entities.Playlist;

namespace MusicApp.Infrastructure.Persistence.Configurations.Playlist;

public class PlaylistTrackConfiguration : IEntityTypeConfiguration<PlaylistTrack>
{
    public void Configure(EntityTypeBuilder<PlaylistTrack> b)
    {
        b.HasKey(pt => new { pt.PlaylistId, pt.TrackId });

        b.HasOne(pt => pt.Playlist)
            .WithMany(p => p.PlaylistTracks)
            .HasForeignKey(pt => pt.PlaylistId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(pt => pt.Track)
            .WithMany(t => t.PlaylistTracks)
            .HasForeignKey(pt => pt.TrackId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Property(pt => pt.CreatedAt)
            .IsRequired();

        b.HasQueryFilter(pt => pt.IsActive);
    }
}
