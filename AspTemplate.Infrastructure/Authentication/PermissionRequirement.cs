using AspTemplate.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AspTemplate.Infrastructure.Authentication;

public class PermissionRequirement(Permission[] permissions)
    : IAuthorizationRequirement
{
    public Permission[] Permissions { get; set; } = permissions;
}