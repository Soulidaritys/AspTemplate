using AspTemplate.Core.Enums;
using AspTemplate.Core.Models;

namespace AspTemplate.Core.Interfaces.Repositories;
public interface IUsersRepository
{
    Task Add(User user);
    Task<T?> GetByEmail<T>(string email);
    Task<HashSet<Permission>> GetUserPermissions(Guid userId);
}

public interface IMediaRepository
{
    Task Add(Media media);
}