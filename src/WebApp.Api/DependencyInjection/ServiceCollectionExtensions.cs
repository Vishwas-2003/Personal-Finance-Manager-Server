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
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICRUDBaseService<>), typeof(CRUDBaseService<>));
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IIncomeService, IncomeService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISummaryService, SummaryService>();
        return services;
    }

    public static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        return services;
    }
}
