using System.Linq.Expressions;
using Amazon.Runtime;
using Amazon.S3;
using AspTemplate.Application.Auth;
using AspTemplate.Infrastructure.Authentication;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ExpressionDebugger;
using Mapster.Utils;

namespace AspTemplate.Infrastructure;
public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddMapster();

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var config = new AmazonS3Config
            {
                ServiceURL = configuration["Minio:ServiceUrl"],
                ForcePathStyle = true,
            };
            var accessKey = configuration["Minio:AccessKey"];
            var secretKey = configuration["Minio:SecretKey"];
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            return new AmazonS3Client(credentials, config);
        });

        return services;
    }
}
