using WebApp.Common.Models.Expense;
using WebApp.Common.Models.Income;
using WebApp.Common.Utilities;

namespace UnitTests;

public class SummaryHierarchyUtilityTests
{
    [Fact]
    public void GroupIncomeByCategoryHierarchy_should_group_by_category_type_then_sub_category()
    {
        var entries = new List<IncomeResponseModel>
        {
            TestDataHelper.CreateIncome(1, 100, 1, "Groceries", 1, "Food"),
            TestDataHelper.CreateIncome(2, 50, 2, "Dining Out", 1, "Food"),
            TestDataHelper.CreateIncome(3, 200, 7, "Monthly Salary", 4, "Salary"),
        };

        var sections = SummaryHierarchyUtility.GroupIncomeByCategoryHierarchy(entries);

        Assert.Equal(2, sections.Count);
        Assert.Equal("Food", sections[0].CategoryTypeName);
        Assert.Equal(150, sections[0].SectionTotal);
        Assert.Equal(2, sections[0].SubCategorySections.Count);
        Assert.Equal("Salary", sections[1].CategoryTypeName);
        Assert.Single(sections[1].SubCategorySections);
        Assert.Equal(200, sections[1].SubCategorySections[0].Subtotal);
    }

    [Fact]
    public void SumSectionTotals_should_sum_category_type_section_totals()
    {
        var sections = SummaryHierarchyUtility.GroupIncomeByCategoryHierarchy(
        [
            TestDataHelper.CreateIncome(1, 100, 1, "Groceries", 1, "Food"),
            TestDataHelper.CreateIncome(2, 200, 7, "Monthly Salary", 4, "Salary"),
        ]);

        Assert.Equal(300, SummaryHierarchyUtility.SumSectionTotals(sections));
    }

    [Fact]
    public void GroupExpensesByCategoryHierarchy_should_group_by_category_type_then_sub_category()
    {
        var entries = new List<ExpenseResponseModel>
        {
            TestDataHelper.CreateExpense(1, 80, 1, "Groceries", 1, "Food"),
            TestDataHelper.CreateExpense(2, 20, 2, "Fuel", 2, "Transport"),
        };

        var sections = SummaryHierarchyUtility.GroupExpensesByCategoryHierarchy(entries);

        Assert.Equal(2, sections.Count);
        Assert.Equal("Food", sections[0].CategoryTypeName);
        Assert.Equal(80, sections[0].SectionTotal);
        Assert.Equal("Transport", sections[1].CategoryTypeName);
        Assert.Equal(20, sections[1].SectionTotal);
    }
}
