using WebApp.Common.Models.Budget;
using WebApp.Data.Entities;

namespace WebApp.Api.Services.Interfaces
{
    public interface IBudgetService : ICRUDBaseService<Budget>
    {
        Task<bool> DeleteBudgetById(int budgetId);
        Task<List<BudgetResponseModel>> GetBudgetByUserId(int userId);
    }
}
