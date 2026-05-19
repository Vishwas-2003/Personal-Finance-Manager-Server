using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Summary;
using WebApp.Common.Utilities;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services;

public class SummaryService(IIncomeRepository incomeRepository, IExpenseRepository expenseRepository) : ISummaryService
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
}
