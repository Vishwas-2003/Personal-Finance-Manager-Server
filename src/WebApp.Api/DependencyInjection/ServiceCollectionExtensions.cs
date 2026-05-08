using WebApp.Api.Services;
using WebApp.Api.Services.Interfaces;
using WebApp.Data.Repositories;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICRUDBaseRepository<>), typeof(CRUDBaseRepository<>));
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IIncomeRepository, IncomeRepository>();
        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICRUDBaseService<>), typeof(CRUDBaseService<>));
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IIncomeService, IncomeService>();
        return services;
    }

    public static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        return services;
    }
}
