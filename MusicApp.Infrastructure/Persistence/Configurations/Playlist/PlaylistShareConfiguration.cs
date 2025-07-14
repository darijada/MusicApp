using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicApp.Core.Entities.Playlist;

namespace MusicApp.Infrastructure.Persistence.Configurations.Playlist
{
    public class PlaylistShareConfiguration : IEntityTypeConfiguration<PlaylistShare>
    {
        public void Configure(EntityTypeBuilder<PlaylistShare> b)
        {
            b.HasKey(ps => new { ps.PlaylistId, ps.UserId });

            b.HasOne(ps => ps.Playlist)
                   .WithMany()
                   .HasForeignKey(ps => ps.PlaylistId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            b.HasOne(ps => ps.User)
                   .WithMany()
                   .HasForeignKey(ps => ps.UserId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);
        }
    }
}
