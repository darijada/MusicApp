using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicApp.Core.Entities.Auth;

namespace MusicApp.Infrastructure.Persistence.Configurations.Auth;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> b)
    {
        b.HasKey(rt => rt.Id);

        b.Property(rt => rt.Token)
            .IsRequired();

        b.Property(rt => rt.UserId)
            .IsRequired();

        b.Property(rt => rt.CreatedAt)
            .IsRequired();

        b.Property(rt => rt.Expires)
            .IsRequired();

        b.Property(rt => rt.IsRevoked)
            .IsRequired();

        b.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasQueryFilter(rt => rt.User != null && rt.User.IsActive);
    }
}
