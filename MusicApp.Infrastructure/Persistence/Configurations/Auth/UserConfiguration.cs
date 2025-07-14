using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicApp.Core.Entities.Auth;

namespace MusicApp.Infrastructure.Persistence.Configurations.Auth;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.Property(u => u.UserName)
            .IsRequired();

        b.Property(u => u.Email)
            .IsRequired();

        b.HasIndex(u => u.NormalizedUserName)
            .IsUnique();

        b.HasIndex(u => u.NormalizedEmail)
            .IsUnique();

        b.HasQueryFilter(u => u.IsActive);
    }
}
