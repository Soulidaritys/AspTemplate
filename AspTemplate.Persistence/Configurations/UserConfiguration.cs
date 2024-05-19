using AspTemplate.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspTemplate.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasDefaultValue(null);

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRoleEntity>(
                l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(r => r.RoleId),
                r => r.HasOne<UserEntity>().WithMany().HasForeignKey(u => u.UserId));

        builder.HasMany(u => u.MediaCollection)
            .WithMany(m => m.Users)
            .UsingEntity<UserMediaEntity>(
                l => 
                    l.HasOne<MediaEntity>().WithMany().HasForeignKey(um => um.ObjectKey).HasConstraintName("FK_CT_UM_2"),
                r => 
                    r.HasOne<UserEntity>().WithMany().HasForeignKey(um => um.UserId).HasConstraintName("FK_CT_UM_1")
            );
    }
}

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileEntity>
{
    public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder.HasKey(up => up.UserId);

        builder.HasOne(up => up.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfileEntity>(up => up.UserId);

        //builder.Property(x => x.UserToMediaEntities)
    }
}

public class MediaConfiguration : IEntityTypeConfiguration<MediaEntity>
{
    public void Configure(EntityTypeBuilder<MediaEntity> builder)
    {
        builder.HasKey(m => m.ObjectKey);

        builder.Property(m => m.MediaType)
            .IsRequired();

        builder.Property(m => m.OriginalFileName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.Width)
            .HasDefaultValue(null);

        builder.Property(m => m.Height)
            .HasDefaultValue(null);

        builder.Property(m => m.DurationInMs)
            .HasDefaultValue(null);
    }
}

public partial class UserMediaConfiguration
    : IEntityTypeConfiguration<UserMediaEntity>
{
    public void Configure(EntityTypeBuilder<UserMediaEntity> builder)
    {
        builder.HasKey(r => new { r.ObjectKey, r.UserId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserToMediaChains)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CT_UM_1");

        builder.HasOne(x => x.Media)
            .WithMany(x => x.UserToMediaChains)
            .HasForeignKey(x => x.ObjectKey)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CT_UM_2");
    }
}