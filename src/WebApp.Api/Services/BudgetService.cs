using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Budget;
using WebApp.Common.Utilities;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services;

public class BudgetService : CRUDBaseService<Budget>, IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository) : base(budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<List<BudgetResponseModel>> GetBudgetByUserId(int userId, BudgetListFilter? filter = null)
    {
        var budgets = await _budgetRepository.GetBudgetByUserId(userId);

        if (filter?.CategoryId is int categoryId)
        {
            budgets = budgets.Where(b => b.Category.Id == categoryId).ToList();
        }

        var keywords = KeywordFilterUtility.SplitKeywords(filter?.Keywords).ToList();
        if (keywords.Count > 0)
        {
            budgets = budgets.Where(b => KeywordFilterUtility.MatchesAnyKeyword(
                [
                    b.Id.ToString(),
                    b.LimitAmount.ToString("0.##"),
                    b.SpentAmount.ToString("0.##"),
                    b.Category.Name,
                    b.Category.CategoryType,
                    b.UpdatedAtUtc.ToString("yyyy-MM-dd"),
                    b.InActive ? "archived" : "active"
                ],
                keywords)).ToList();
        }

        return budgets
            .OrderByDescending(b => b.UpdatedAtUtc)
            .ThenByDescending(b => b.Id)
            .ToList();
    }

    public async Task<bool> DeleteBudgetById(int budgetId) => await DeleteAsync(budgetId);

    public async Task<bool> UpdateBudgetById(int budgetId, BudgetModel model)
    {
        var budget = await ReadAsync(budgetId);
        if (budget is null || budget.InActive)
        {
            return false;
        }

        budget.CategoryId = model.CategoryId;
        budget.LimitAmount = model.LimitAmount;
        budget.SpentAmount = model.SpentAmount;
        budget.UpdatedAtUtc = DateTime.UtcNow;
        await UpdateAsync(budget);
        return true;
    }
}
