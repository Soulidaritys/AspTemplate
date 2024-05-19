using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspTemplate.Application.Auth;
using AspTemplate.Application.Models.Auth;
using AspTemplate.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AspTemplate.Infrastructure.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public AppAuthToken Generate(User user)
    {
        List<Claim> claims =
        [
            new Claim(CustomClaims.UserId, user.Id.ToString()),
        ];

        if(user.Roles is not null) 
            claims.AddRange(user.Roles.Select(role => new Claim(CustomClaims.Roles, role.ToString())));

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours),
            signingCredentials: signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return new AppAuthToken()
        {
            Token = tokenValue,
            Expires = token.ValidTo,
        };
    }
}
