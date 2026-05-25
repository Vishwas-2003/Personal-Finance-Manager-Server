namespace WebApp.Common.Models.Income;

public class IncomeListFilter
{
    public int? CategoryId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Keywords { get; set; }
}
