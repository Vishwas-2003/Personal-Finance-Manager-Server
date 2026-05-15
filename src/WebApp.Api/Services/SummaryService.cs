using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Summary;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services
{
    public class SummaryService(IIncomeRepository _incomeRepository, IExpenseRepository _expenseRepository) : ISummaryService
    {
        public async Task<IncomeSummaryResponseModel> GetTotalIncomeSummary(int userId)
        {
            var incomes = await _incomeRepository.GetIncomeByUserId(userId);

            var summary = new IncomeSummaryResponseModel
            {
                Incomes = incomes,
                TotalIncome = incomes.Sum(income => income.Amount)
            };

            return summary;
        }

        public async Task<ExpenseSummaryResponseModel> GetTotalExpenseSummary(int userId)
        {
            var expenses = await _expenseRepository.GetExpensesByUserId(userId);

            var summary = new ExpenseSummaryResponseModel
            {
                Expenses = expenses,
                TotalExpense = expenses.Sum(income => income.Amount)
            };

            return summary;
        }
    }
}
