using AspTemplate.Core;
using Microsoft.AspNetCore.Mvc;
using Sstv.DomainExceptions.DebugViewer;
using Sstv.DomainExceptions.Extensions.DependencyInjection;
using Sstv.DomainExceptions;
using System.Diagnostics;
using Sstv.DomainExceptions.Extensions.ProblemDetails;

namespace AspTemplate.API.ProblemDetails;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// TraceId problem details extensions name.
    /// </summary>
    private const string TRACE_ID_KEY = "traceId";

    /// <summary>
    /// Default error code.
    /// </summary>
    private static readonly ErrorDescription _defaultErrorCodeDescription =
        ErrorCodes.Default.GetDescription();

    /// <summary>
    /// Default problem details response.
    /// </summary>
    private static readonly ErrorCodeProblemDetails _defaultErrorResponse = new(_defaultErrorCodeDescription);

    public static IServiceCollection AddBusinessException(this IServiceCollection services)
    {
        return services.AddDomainExceptions(o =>
        {
            o.WithErrorCodesDescriptionSource(BusinessException.ErrorCodesDescriptionSource);
            o.UseDomainExceptionHandler();
            o.ConfigureSettings = (sp, settings) =>
            {
                var exceptionLogger = sp.GetRequiredService<ILogger<DomainException>>();

                settings.OnExceptionCreated += exception =>
                {
                    var logLevel = ErrorCodeMapping.IsError(exception.ErrorCode)
                        ? LogLevel.Error
                        : LogLevel.Information;

                    //exceptionLogger.LogDomainException(logLevel, exception, exception.ErrorCode, exception.Message);
                };
            };
        });
    }

    public static IServiceCollection AddBusinessProblemDetails(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDomainExceptionDebugEnricher, StatusCodeEnricher>()
            .AddProblemDetails(o =>
            {
                o.CustomizeProblemDetails = ctx =>
                {
                    if (ctx.Exception is DomainException de)
                    {
                        ctx.ProblemDetails.Status =
                            ctx.HttpContext.Response.StatusCode =
                                ErrorCodeMapping.MapToStatusCode(de.ErrorCode);
                    }
                    else
                    {
                        ctx.ProblemDetails = _defaultErrorResponse;
                        ctx.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        ErrorCodesMeter.Measure(_defaultErrorCodeDescription);
                    }

                    var addExceptionDetails = !ctx.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>()
                        .IsProduction();

                    if (addExceptionDetails && ctx.Exception is not null)
                    {
                        ctx.ProblemDetails.Extensions["exceptionDetails"] = ctx.Exception.ToString();
                    }

                    if (!ctx.ProblemDetails.Extensions.ContainsKey(TRACE_ID_KEY))
                    {
                        var traceId = Activity.Current?.Id ?? ctx.HttpContext.TraceIdentifier;

                        if (!string.IsNullOrWhiteSpace(traceId))
                        {
                            ctx.ProblemDetails.Extensions[TRACE_ID_KEY] = traceId;
                        }
                    }
                };
            });
    }

    /// <summary>
    /// Configures API behavior to return ProblemDetails on validation.
    /// </summary>
    public static IMvcBuilder AddValidationProblemDetails(this IMvcBuilder builder)
    {
        return builder.ConfigureApiBehaviorOptions(o =>
        {
            o.InvalidModelStateResponseFactory = context =>
            {
                var errorDescription = ErrorCodes.InvalidData.GetDescription();
                ErrorCodesMeter.Measure(errorDescription);
                var statusCode = ErrorCodeMapping.MapToStatusCode(errorDescription.ErrorCode);

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Title = errorDescription.Description,
                    Type = !string.IsNullOrWhiteSpace(errorDescription.HelpLink)
                        ? errorDescription.HelpLink
                        : $"https://httpstatuses.io/{statusCode}",
                    Extensions =
                    {
                        ["code"] = errorDescription.ErrorCode,
                        [TRACE_ID_KEY] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
                    }
                };

                return new ObjectResult(problemDetails)
                {
                    StatusCode = statusCode
                };
            };
        });
    }
}