using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Api.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        return services;
    }
}
