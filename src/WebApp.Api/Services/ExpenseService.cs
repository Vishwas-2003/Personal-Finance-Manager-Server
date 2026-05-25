using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Expense;
using WebApp.Common.Utilities;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services;

public class ExpenseService : CRUDBaseService<Expense>, IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;

    public ExpenseService(IExpenseRepository expenseRepository) : base(expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public override async Task<Expense> CreateAsync(Expense entity)
    {
        TransactionDateValidator.EnsureNotFuture(entity.Date);
        return await base.CreateAsync(entity);
    }

    public async Task<List<ExpenseResponseModel>> GetExpensesByUserId(int userId, ExpenseListFilter? filter = null)
    {
        var expenses = await _expenseRepository.GetExpensesByUserId(userId);

        if (filter?.CategoryId is int categoryId)
        {
            expenses = expenses.Where(e => e.Category.Id == categoryId).ToList();
        }

        if (filter?.FromDate is DateTime fromDate)
        {
            expenses = expenses.Where(e => e.Date.Date >= fromDate.Date).ToList();
        }

        if (filter?.ToDate is DateTime toDate)
        {
            expenses = expenses.Where(e => e.Date.Date <= toDate.Date).ToList();
        }

        var keywords = KeywordFilterUtility.SplitKeywords(filter?.Keywords).ToList();
        if (keywords.Count > 0)
        {
            expenses = expenses.Where(e => KeywordFilterUtility.MatchesAnyKeyword(
                [
                    e.Id.ToString(),
                    e.Amount.ToString("0.##"),
                    e.Category.Name,
                    e.Category.CategoryType,
                    e.Description ?? string.Empty,
                    e.Date.ToString("yyyy-MM-dd"),
                    e.InActive ? "archived" : "active"
                ],
                keywords)).ToList();
        }

        return expenses
            .OrderByDescending(e => e.Date)
            .ThenByDescending(e => e.Id)
            .ToList();
    }

    public async Task<bool> DeleteExpenseById(int expenseId) => await DeleteAsync(expenseId);

    public async Task<bool> UpdateExpenseById(int expenseId, ExpenseModel model)
    {
        TransactionDateValidator.EnsureNotFuture(model.Date);

        var expense = await ReadAsync(expenseId);
        if (expense is null || expense.InActive)
        {
            return false;
        }

        expense.Amount = model.Amount;
        expense.CategoryId = model.CategoryId;
        expense.Description = model.Description;
        expense.Date = model.Date;
        await UpdateAsync(expense);
        return true;
    }
}
