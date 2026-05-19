namespace WebApp.Common.Models.Summary;

public class IncomeSummaryResponseModel
{
    public List<IncomeSummaryCategoryTypeSection> CategoryTypeSections { get; set; } = [];
    public decimal TotalIncome { get; set; }
}
