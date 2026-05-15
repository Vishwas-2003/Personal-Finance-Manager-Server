namespace WebApp.Common.Models.Summary;

public class IncomeSummaryCategoryTypeSection
{
    public int CategoryTypeId { get; set; }
    public required string CategoryTypeName { get; set; }
    public decimal SectionTotal { get; set; }
    public List<IncomeSummarySubCategorySection> SubCategorySections { get; set; } = [];
}
