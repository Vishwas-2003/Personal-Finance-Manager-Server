using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Budget;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services
{
    public class BudgetService : CRUDBaseService<Budget>, IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetService(IBudgetRepository budgetRepository) : base(budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<List<BudgetResponseModel>> GetBudgetByUserId(int userId)
        {
            return await _budgetRepository.GetBudgetByUserId(userId);
        }

        public async Task<bool> DeleteBudgetById(int budgetId)
        {
            return await DeleteAsync(budgetId);
        }
    }
}
