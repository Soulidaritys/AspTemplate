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
        string securityStamp,
        string? firstName,
        string? lastName,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt,
        IReadOnlyCollection<Role>? roles,
        IReadOnlyCollection<Media>? media)
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
        SecurityStamp = securityStamp;
        FirstName = firstName;
        LastName = lastName;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Roles = roles;
        Media = media;
    }

    public UserId Id { get; private set; }
    public string Email { get; private set; }
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public string SecurityStamp { get; private set; }

    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Role>? Roles { get; private set; }
    public IReadOnlyCollection<Media>? Media { get; private set; }
}