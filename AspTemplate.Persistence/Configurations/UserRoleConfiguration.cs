using AspTemplate.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspTemplate.Persistence.Configurations;

public partial class UserRoleConfiguration
    : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.HasKey(r => new { r.UserId, r.RoleId });
    }
}