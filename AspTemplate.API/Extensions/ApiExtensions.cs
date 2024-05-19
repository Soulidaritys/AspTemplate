using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using AspTemplate.API.ProblemDetails;
using AspTemplate.Application.Services;
using AspTemplate.Core;
using AspTemplate.Core.Enums;
using AspTemplate.Core.Interfaces.Services;
using AspTemplate.Infrastructure.Authentication;
using ExpressionDebugger;
using Mapster;
using Mapster.Utils;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Sstv.DomainExceptions;
using Sstv.DomainExceptions.Extensions.DependencyInjection;
using Sstv.DomainExceptions.Extensions.ProblemDetails;

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
