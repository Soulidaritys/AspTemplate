using AspTemplate.Core.Enums;

namespace AspTemplate.Core.Interfaces.Services;

public interface IPermissionService
{
    Task<HashSet<Permission>> GetPermissionsAsync(Guid userId);
}
