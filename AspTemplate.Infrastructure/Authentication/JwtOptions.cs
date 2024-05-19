namespace AspTemplate.Infrastructure.Authentication;

public class JwtOptions
{
    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public string SecretKey { get; set; } = string.Empty;

    public int ExpiresHours { get; set; }
}
