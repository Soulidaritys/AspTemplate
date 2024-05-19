using AspTemplate.Application.Models.Auth;
using AspTemplate.Core.Models;

namespace AspTemplate.Application.Auth;
public interface IJwtProvider
{
    AppAuthToken Generate(User user);
}