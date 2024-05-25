using AspTemplate.Application.Dto;
using AspTemplate.Core.Enums;
using AspTemplate.Core.Models;

namespace AspTemplate.Core.Interfaces.Repositories;
public interface IUsersRepository
{
    Task Add(User user);
    Task<User?> GetByEmail(string email);
    Task<UserJwtValidateDto?> GetUserJwtValidateDto(UserId userId);
    Task<HashSet<Permission>> GetUserPermissions(Guid userId);
}

public interface IMediaRepository
{
    Task Add(Media media);
}