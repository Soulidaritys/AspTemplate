using AspTemplate.Application.Services;
using AspTemplate.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AspTemplate.Application;
public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
