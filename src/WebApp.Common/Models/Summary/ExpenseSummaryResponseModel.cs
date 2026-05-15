using WebApp.Common.Models.Expense;

namespace WebApp.Common.Models.Summary
{
    public class ExpenseSummaryResponseModel
    {
        public List<ExpenseResponseModel> Expenses { get; set; } = new List<ExpenseResponseModel>();
        public decimal TotalExpense { get; set; }
    }
}
