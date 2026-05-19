namespace WebApp.Common.Models.Summary;

public class BalanceSummaryResponseModel
{
    public List<BalanceSummaryCreditLine> Credits { get; set; } = [];
    public List<BalanceSummaryDebitLine> Debits { get; set; } = [];
    public decimal TotalCredit { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal Balance { get; set; }
}

public class BalanceSummaryCreditLine
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Source { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryType { get; set; } = string.Empty;
}

public class BalanceSummaryDebitLine
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryType { get; set; } = string.Empty;
}
