using AspTemplate.Persistence.Configurations;
using AspTemplate.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AspTemplate.Persistence;

public class AppDbContext : DbContext
{
    private readonly IOptions<AuthorizationOptions> _authOptions;

    public AppDbContext(DbContextOptions<AppDbContext> options,
        IOptions<AuthorizationOptions> authOptions) : base(options)
    {
        _authOptions = authOptions;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
    }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<RoleEntity> Roles { get; set; }

    public DbSet<MediaEntity> Media { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(_authOptions.Value));
    }
}