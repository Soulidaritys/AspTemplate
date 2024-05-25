using AspTemplate.Core.Enums;

namespace AspTemplate.Application.Models.Users;

public class RegisterUserModel
{
    public RegisterUserModel(
        string email,
        string userName,
        string password,
        IReadOnlyCollection<Role> roles,
        string? firstName,
        string? lastName)
    {
        Email = email;
        UserName = userName;
        Password = password;
        Roles = roles;
        FirstName = firstName;
        LastName = lastName;
    }

    public string Email { get; private set; }
    public string UserName { get; private set; }
    public string Password { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public IReadOnlyCollection<Role> Roles { get; private set; }
}