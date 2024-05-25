using AspTemplate.Core.Enums;
using AspTemplate.Infrastructure.Authentication;

namespace AspTemplate.API.Extensions;

public static class ApiExtensions
{
    public static IEndpointConventionBuilder RequirePermissions<TBuilder>(
        this TBuilder builder, params Permission[] permissions)
            where TBuilder : IEndpointConventionBuilder
    {
        return builder
            .RequireAuthorization(pb =>
                pb.AddRequirements(new PermissionRequirement(permissions)));
    }
}
