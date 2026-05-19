using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Expense;
using WebApp.Common.Models.Income;
using WebApp.Common.Models.Summary;
using WebApp.Common.Utilities;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services;

public class SummaryService(
    IIncomeRepository incomeRepository,
    IExpenseRepository expenseRepository,
    IIncomeService incomeService,
    IExpenseService expenseService) : ISummaryService
{
    public async Task<IncomeSummaryResponseModel> GetTotalIncomeSummary(int userId)
    {
        var incomeEntries = await incomeRepository.GetIncomeByUserId(userId);
        var categoryTypeSections = SummaryHierarchyUtility.GroupIncomeByCategoryHierarchy(incomeEntries);

        return new IncomeSummaryResponseModel
        {
            CategoryTypeSections = categoryTypeSections,
            TotalIncome = SummaryHierarchyUtility.SumSectionTotals(categoryTypeSections)
        };
    }

    public async Task<ExpenseSummaryResponseModel> GetTotalExpenseSummary(int userId)
    {
        var expenseEntries = await expenseRepository.GetExpensesByUserId(userId);
        var categoryTypeSections = SummaryHierarchyUtility.GroupExpensesByCategoryHierarchy(expenseEntries);

        return new ExpenseSummaryResponseModel
        {
            CategoryTypeSections = categoryTypeSections,
            TotalExpense = SummaryHierarchyUtility.SumSectionTotals(categoryTypeSections)
        };
    }

    public async Task<BalanceSummaryResponseModel> GetBalanceSummary(int userId, BalanceSummaryFilter? filter = null)
    {
        var incomes = ApplyDateFilter(await incomeService.GetIncomeByUserId(userId), filter);
        var expenses = await expenseService.GetExpensesByUserId(userId, ToExpenseFilter(filter));

        var credits = incomes
            .Select(i => new BalanceSummaryCreditLine
            {
                Id = i.Id,
                Amount = i.Amount,
                Date = i.Date,
                Source = i.Source,
                Notes = i.Notes,
                CategoryName = i.Category.Name,
                CategoryType = i.Category.CategoryType
            })
            .OrderByDescending(c => c.Date)
            .ThenByDescending(c => c.Id)
            .ToList();

        var debits = expenses
            .Select(e => new BalanceSummaryDebitLine
            {
                Id = e.Id,
                Amount = e.Amount,
                Date = e.Date,
                Description = e.Description,
                CategoryName = e.Category.Name,
                CategoryType = e.Category.CategoryType
            })
            .OrderByDescending(d => d.Date)
            .ThenByDescending(d => d.Id)
            .ToList();

        var totalCredit = credits.Sum(c => c.Amount);
        var totalDebit = debits.Sum(d => d.Amount);

        return new BalanceSummaryResponseModel
        {
            Credits = credits,
            Debits = debits,
            TotalCredit = totalCredit,
            TotalDebit = totalDebit,
            Balance = totalCredit - totalDebit
        };
    }

    private static ExpenseListFilter? ToExpenseFilter(BalanceSummaryFilter? filter) =>
        filter is null ? null : new ExpenseListFilter { FromDate = filter.FromDate, ToDate = filter.ToDate };

    private static List<IncomeResponseModel> ApplyDateFilter(
        List<IncomeResponseModel> incomes,
        BalanceSummaryFilter? filter)
    {
        if (filter?.FromDate is DateTime fromDate)
        {
            incomes = incomes.Where(i => i.Date.Date >= fromDate.Date).ToList();
        }

        if (filter?.ToDate is DateTime toDate)
        {
            incomes = incomes.Where(i => i.Date.Date <= toDate.Date).ToList();
        }

        return incomes;
    }
}
