using AspTemplate.API.Extensions;
using AspTemplate.Application;
using AspTemplate.Infrastructure;
using AspTemplate.Infrastructure.Authentication;
using AspTemplate.Persistence;
using Microsoft.AspNetCore.CookiePolicy;
using Sstv.DomainExceptions.Extensions.DependencyInjection;
using AspTemplate.API.ProblemDetails;
using AspTemplate.API.Swagger;

namespace AspTemplate.API;

public class Startup(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;


    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<JwtOptions>(_configuration.GetSection(nameof(JwtOptions)))
            .Configure<AuthorizationOptions>(_configuration.GetSection(nameof(AuthorizationOptions)));

        services.AddSwagger();
        services.AddMapster();

        services
            .AddApiControllers()
            .AddApiAuthentication(_configuration);

        services
            .AddBusinessException()
            .AddPersistence(_configuration)
            .AddApplication()
            .AddInfrastructure(_configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseErrorCodesDebugView();
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always,
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpointBuilder =>
        {
            endpointBuilder.MapControllers().RequireAuthorization();
            endpointBuilder.MapSwaggerUI(app);
            //endpointBuilder.MapSwagger();
        });
    }
}