using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaylistEntity = MusicApp.Core.Entities.Playlist.Playlist;

namespace MusicApp.Infrastructure.Persistence.Configurations.Playlist;

public class PlaylistConfiguration : IEntityTypeConfiguration<PlaylistEntity>
{
    public void Configure(EntityTypeBuilder<PlaylistEntity> b)
    {
        b.HasKey(p => p.Id);

        b.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        b.HasOne(p => p.Owner)
            .WithMany() // empty because no navigation collection in User entity
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Property(p => p.CreatedAt)
            .IsRequired();

        b.HasQueryFilter(p => p.IsActive);
    }
}
