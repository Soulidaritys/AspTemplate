namespace AspTemplate.Application.Models.Auth;

public class AppAuthToken
{
    public string Token { get; init; }
    public DateTimeOffset Expires { get; init; }
}