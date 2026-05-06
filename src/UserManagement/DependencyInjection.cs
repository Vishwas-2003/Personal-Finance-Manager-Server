using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Configuration;
using UserManagement.Services;
using UserManagement.Services.Interfaces;
using WebApp.Data.Entities;
using WebApp.Data.Repositories;
using WebApp.Data.Repositories.Interfaces;

namespace UserManagement;

public static class DependencyInjection
{
    public static IServiceCollection RegisterTenantRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IServiceCollection RegisterTenantServices(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(DependencyInjection).Assembly,
            typeof(WebApp.Data.DependencyInjection).Assembly);
        services
            .AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .Validate(
                x => !string.IsNullOrWhiteSpace(x.Secret)
                    && !string.IsNullOrWhiteSpace(x.Issuer)
                    && !string.IsNullOrWhiteSpace(x.Audience),
                "Jwt configuration is missing required values.");

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }

    public static IServiceCollection RegisterTenantControllers(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddUserManagement(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = configuration;

        services.RegisterTenantRepositories();
        services.RegisterTenantServices();
        services.RegisterTenantControllers();

        return services;
    }
}
