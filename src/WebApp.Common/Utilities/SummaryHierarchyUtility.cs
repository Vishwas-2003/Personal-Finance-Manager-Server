using WebApp.Common.Models.Expense;
using WebApp.Common.Models.Income;
using WebApp.Common.Models.Summary;

namespace WebApp.Common.Utilities;

/// <summary>
/// Builds hierarchical summary structures grouped by category type, then sub-category.
/// </summary>
public static class SummaryHierarchyUtility
{
    public static List<IncomeSummaryCategoryTypeSection> GroupIncomeByCategoryHierarchy(
        IEnumerable<IncomeResponseModel> incomeEntries)
    {
        return incomeEntries
            .GroupBy(entry => new { entry.Category.CategoryTypeId, entry.Category.CategoryType })
            .OrderBy(categoryTypeGroup => categoryTypeGroup.Key.CategoryType)
            .Select(categoryTypeGroup => new IncomeSummaryCategoryTypeSection
            {
                CategoryTypeId = categoryTypeGroup.Key.CategoryTypeId,
                CategoryTypeName = categoryTypeGroup.Key.CategoryType,
                SectionTotal = categoryTypeGroup.Sum(entry => entry.Amount),
                SubCategorySections = categoryTypeGroup
                    .GroupBy(entry => new { entry.Category.Id, entry.Category.Name })
                    .OrderBy(subCategoryGroup => subCategoryGroup.Key.Name)
                    .Select(subCategoryGroup => new IncomeSummarySubCategorySection
                    {
                        CategoryId = subCategoryGroup.Key.Id,
                        CategoryName = subCategoryGroup.Key.Name,
                        Subtotal = subCategoryGroup.Sum(entry => entry.Amount),
                        IncomeEntries = subCategoryGroup
                            .OrderByDescending(entry => entry.Date)
                            .ToList()
                    })
                    .ToList()
            })
            .ToList();
    }

    public static List<ExpenseSummaryCategoryTypeSection> GroupExpensesByCategoryHierarchy(
        IEnumerable<ExpenseResponseModel> expenseEntries)
    {
        return expenseEntries
            .GroupBy(entry => new { entry.Category.CategoryTypeId, entry.Category.CategoryType })
            .OrderBy(categoryTypeGroup => categoryTypeGroup.Key.CategoryType)
            .Select(categoryTypeGroup => new ExpenseSummaryCategoryTypeSection
            {
                CategoryTypeId = categoryTypeGroup.Key.CategoryTypeId,
                CategoryTypeName = categoryTypeGroup.Key.CategoryType,
                SectionTotal = categoryTypeGroup.Sum(entry => entry.Amount),
                SubCategorySections = categoryTypeGroup
                    .GroupBy(entry => new { entry.Category.Id, entry.Category.Name })
                    .OrderBy(subCategoryGroup => subCategoryGroup.Key.Name)
                    .Select(subCategoryGroup => new ExpenseSummarySubCategorySection
                    {
                        CategoryId = subCategoryGroup.Key.Id,
                        CategoryName = subCategoryGroup.Key.Name,
                        Subtotal = subCategoryGroup.Sum(entry => entry.Amount),
                        ExpenseEntries = subCategoryGroup
                            .OrderByDescending(entry => entry.Date)
                            .ToList()
                    })
                    .ToList()
            })
            .ToList();
    }

    public static decimal SumSectionTotals(IEnumerable<IncomeSummaryCategoryTypeSection> sections) =>
        sections.Sum(section => section.SectionTotal);

    public static decimal SumSectionTotals(IEnumerable<ExpenseSummaryCategoryTypeSection> sections) =>
        sections.Sum(section => section.SectionTotal);
}
