using WebApp.Common.Models.Income;

namespace WebApp.Common.Models.Summary;

public class IncomeSummarySubCategorySection
{
    public int CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public decimal Subtotal { get; set; }
    public List<IncomeResponseModel> IncomeEntries { get; set; } = [];
}
