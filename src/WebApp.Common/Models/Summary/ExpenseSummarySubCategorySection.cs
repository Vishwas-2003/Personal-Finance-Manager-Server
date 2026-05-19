using WebApp.Common.Models.Expense;

namespace WebApp.Common.Models.Summary;

public class ExpenseSummarySubCategorySection
{
    public int CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public decimal Subtotal { get; set; }
    public List<ExpenseResponseModel> ExpenseEntries { get; set; } = [];
}
