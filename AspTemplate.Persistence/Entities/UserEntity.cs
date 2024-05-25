using AspTemplate.Core.Enums;
using AspTemplate.Core.Models;

namespace AspTemplate.Persistence.Entities;
public class UserEntity
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string SecurityStamp { get; set; } = string.Empty;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }


    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public ICollection<RoleEntity> Roles { get; set; } = [];
    public ICollection<MediaEntity> MediaCollection { get; set; } = [];
    
    public ICollection<UserMediaEntity> UserToMediaChains { get; set; } = [];
}

public class UserMediaEntity
{
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    public string ObjectKey { get; set; }
    public MediaEntity? Media { get; set; }
}

public class MediaEntity
{
    public string ObjectKey { get; set; }
    public MediaType MediaType { get; set; }
    public string OriginalFileName { get; set; }

    public Guid? CreatedByUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }


    public int? Width { get; set; }
    public int? Height { get; set; }

    public uint? DurationInMs { get; set; }

    public ICollection<UserMediaEntity> UserToMediaChains { get; set; } = [];
    public ICollection<UserEntity> Users { get; set; } = [];
}