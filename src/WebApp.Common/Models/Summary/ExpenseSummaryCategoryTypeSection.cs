namespace WebApp.Common.Models.Summary;

public class ExpenseSummaryCategoryTypeSection
{
    public int CategoryTypeId { get; set; }
    public required string CategoryTypeName { get; set; }
    public decimal SectionTotal { get; set; }
    public List<ExpenseSummarySubCategorySection> SubCategorySections { get; set; } = [];
}
