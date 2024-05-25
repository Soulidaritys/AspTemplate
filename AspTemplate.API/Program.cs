using System.Net;
using AspTemplate.Persistence.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Sstv.DomainExceptions.Extensions.SerilogEnricher;

namespace AspTemplate.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(new JsonFormatter())
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting AspTemplate");

            var host = CreateHostBuilder(args).Build();

            await host.Services.MigrateDatabase();

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "AspTemplate terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseStartup<Startup>()
                    .UseShutdownTimeout(TimeSpan.FromSeconds(10));
            })
            .UseSerilog((ctx, c) =>
            {
                c.ReadFrom.Configuration(ctx.Configuration);
                c.WriteTo.Console(new JsonFormatter());
                c.Enrich.FromLogContext();
                c.Enrich.WithProperty("Host", ctx.Configuration.GetValue("HOSTNAME", Dns.GetHostName()));
                // add here env etc
                c.Enrich.WithDomainException();
            });
}


#region OldProgramCs

//var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog((context, loggerConfig) =>
//    loggerConfig.ReadFrom.Configuration(context.Configuration));

//var services = builder.Services;
//var configuration = builder.Configuration;

//services.AddApiAuthentication(configuration);

//services.AddEndpointsApiExplorer();

//services.AddSwaggerGen();

//services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
//services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

//services
//    .AddMapster()
//    .AddPersistence(configuration)
//    .AddApplication()
//    .AddInfrastructure(configuration);

//builder.Services.AddProblemDetails();
//services.AddExceptionHandler<GlobalExceptionHandler>();


//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseExceptionHandler();

//app.UseMiddleware<RequestLogContextMiddleware>();

//app.UseSerilogRequestLogging();

//app.UseCookiePolicy(new CookiePolicyOptions
//{
//    MinimumSameSitePolicy = SameSiteMode.Strict,
//    HttpOnly = HttpOnlyPolicy.Always,
//    Secure = CookieSecurePolicy.Always
//});

//app.UseAuthentication();

//app.UseAuthorization();

//app.AddMappedEndpoints();

//app.MapGet("get", () =>
//{
//    return Results.Ok("ok");
//}).RequireAuthorization("AdminPolicy");

//await app.Services.MigrateDatabase();
//app.Run();

#endregion
