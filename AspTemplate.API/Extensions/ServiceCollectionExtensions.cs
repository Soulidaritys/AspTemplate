using System.Security.Claims;
using AspTemplate.Application.Services;
using AspTemplate.Core.Enums;
using AspTemplate.Core.Interfaces.Services;
using AspTemplate.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AspTemplate.API.ProblemDetails;
using System.Text.Json;
using System.Text.Json.Serialization;
using AspTemplate.Core.Interfaces.Repositories;
using AspTemplate.Core.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AspTemplate.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(o => { o.DisableDataAnnotationsValidation = true; })
            .AddBusinessProblemDetails()
            .AddControllers(o =>
            {
                o.OutputFormatters.RemoveType<StringOutputFormatter>();
                o.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            })
            .AddControllersAsServices()
            .AddValidationProblemDetails()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                o.AllowInputFormatterExceptionMessages = false;
            });

        return services;
    }

    public static IServiceCollection AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["secretCookie"];

                        return Task.CompletedTask;
                    },

                    OnTokenValidated = async (context) =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUsersRepository>();
                        if(!Guid.TryParse(context.Principal?.FindFirstValue(CustomClaims.UserId), out var userIdGuid))
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        var user = await userService.GetUserJwtValidateDto(new UserId(userIdGuid));
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        if (context.Principal.FindFirstValue(CustomClaims.SecurityStamp) != user.SecurityStamp)
                            context.Fail("Unauthorized");
                    },
                };
            });

        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyConstants.AdminPolicy, policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireRole(Role.Admin.ToString());
            });

            options.AddPolicy(PolicyConstants.ExecutorUserPolicy, policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireRole(Role.ExecutorUser.ToString());
            });

            options.AddPolicy(PolicyConstants.ConsumerUserPolicy, policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireRole(Role.ConsumerUser.ToString());
            });
        });

        return services;
    }
}