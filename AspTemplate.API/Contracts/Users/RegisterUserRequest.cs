using System.ComponentModel.DataAnnotations;
using AspTemplate.Application.Models.Users;
using AspTemplate.Core.Enums;

namespace AspTemplate.API.Contracts.Users;

public record RegisterUserRequest(
    [Required] string UserName,
    [Required] string Password,
    [Required] string Email,
    [Required] Role Role)
{
    public RegisterUserModel ToModel()
    {
        return new RegisterUserModel(
            UserName, 
            Password, 
            Email, 
            new[] { Role }, 
            null, 
            null);
    }
}