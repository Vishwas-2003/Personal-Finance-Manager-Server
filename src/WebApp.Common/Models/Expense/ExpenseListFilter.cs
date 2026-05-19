namespace WebApp.Common.Models.Expense;

public class ExpenseListFilter
{
    public int? CategoryId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
