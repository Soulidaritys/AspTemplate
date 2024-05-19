using AspTemplate.Application.Models.Auth;
using AspTemplate.Application.Models.Users;
using AspTemplate.Core.Enums;
using AspTemplate.Core.Models;

namespace AspTemplate.Core.Interfaces.Services;

public interface IUserService
{
    Task<AppAuthToken> Login(string email, string password);
    Task Register(RegisterUserModel registerUserModel);
    Task<CreateDefaultUsersResult> CreateDefaultUsers();
    Task<User> GetUserByEmail(string email);
}