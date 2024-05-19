using AspTemplate.Application.Auth;

namespace AspTemplate.Infrastructure.Authentication;
public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
        => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool Verify(string password, string hashedPassword)
        => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
}