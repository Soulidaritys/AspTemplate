namespace AspTemplate.Core.Models;

public class UserProfile
{
    public UserProfile(
        UserId userId,
        string? firstName,
        string? lastName,
        Media? avatar,
        DateTimeOffset? updatedAt = null)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Avatar = avatar;
        UpdatedAt = updatedAt;
    }
    public UserId UserId { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public Media? Avatar { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }
}