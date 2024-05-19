using AspTemplate.Core.Enums;

namespace AspTemplate.Application.Models.Users;

public class RegisterUserModel
{
    private RegisterUserModel() { }

    public string Email { get; private set; }
    public string UserName { get; private set; }
    public string Password { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public IReadOnlyCollection<Role> Roles { get; private set; }

    public static RegisterUserModel Create(
        string email,
        string userName,
        string password,
        IReadOnlyCollection<Role> roles,
        string? firstName,
        string? lastName)
    {
        var user = new RegisterUserModel
        {
            Email = email,
            UserName = userName,
            Password = password,
            Roles = roles,
            FirstName = firstName,
            LastName = lastName,
        };

        return user;
    }
}