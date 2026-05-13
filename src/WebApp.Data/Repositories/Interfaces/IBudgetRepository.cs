using WebApp.Common.Models.Budget;
using WebApp.Data.Entities;

namespace WebApp.Data.Repositories.Interfaces
{
    public interface IBudgetRepository : ICRUDBaseRepository<Budget>
    {
        Task<List<BudgetResponseModel>> GetBudgetByUserId(int userId);
    }
}
