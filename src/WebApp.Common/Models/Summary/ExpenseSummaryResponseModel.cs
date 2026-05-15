namespace WebApp.Common.Models.Summary;

public class ExpenseSummaryResponseModel
{
    public List<ExpenseSummaryCategoryTypeSection> CategoryTypeSections { get; set; } = [];
    public decimal TotalExpense { get; set; }
}
