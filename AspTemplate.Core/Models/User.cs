using AspTemplate.Core.Enums;

namespace AspTemplate.Core.Models;

[StronglyTypedId(
    backingType: StronglyTypedIdBackingType.Guid,
    jsonConverter: StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct UserId { }

public class User
{
    public User(
        UserId id,
        string userName,
        string passwordHash,
        string email,
        UserProfile? userProfile,
        IReadOnlyCollection<Role>? roles,
        IReadOnlyCollection<Media>? media,
        DateTimeOffset? createdAt = null,
        DateTimeOffset? updatedAt = null)
    {
        if(string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(userName));
        if(string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(passwordHash));
        if(string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));

        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        CreatedAt = createdAt ?? DateTimeOffset.UtcNow;
        UpdatedAt = updatedAt;
        UserProfile = userProfile;
        Roles = roles;
    }

    public UserId Id { get; private set; }
    public string Email { get; private set; }
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public UserProfile? UserProfile { get; private set; }
    public IReadOnlyCollection<Role>? Roles { get; private set; }
    public IReadOnlyCollection<Media>? Media { get; private set; }
}