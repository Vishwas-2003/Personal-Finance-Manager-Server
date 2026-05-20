using WebApp.Common.Models.Category;
using WebApp.Common.Models.Expense;
using WebApp.Common.Models.Income;

namespace UnitTests;

internal static class TestDataHelper
{
    public static CategoryModel CreateCategory(
        int id,
        string name,
        int categoryTypeId,
        string categoryType) =>
        new()
        {
            Id = id,
            Name = name,
            CategoryTypeId = categoryTypeId,
            CategoryType = categoryType,
        };

    public static IncomeResponseModel CreateIncome(
        int id,
        decimal amount,
        int categoryId,
        string categoryName,
        int categoryTypeId,
        string categoryTypeName,
        DateTime? date = null,
        string source = "Test") =>
        new()
        {
            Id = id,
            Amount = amount,
            Source = source,
            Date = date ?? DateTime.UtcNow.Date,
            CreatedAtUtc = DateTime.UtcNow,
            Category = CreateCategory(categoryId, categoryName, categoryTypeId, categoryTypeName),
        };

    public static ExpenseResponseModel CreateExpense(
        int id,
        decimal amount,
        int categoryId,
        string categoryName,
        int categoryTypeId,
        string categoryTypeName,
        DateTime? date = null,
        string? description = null) =>
        new()
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date ?? DateTime.UtcNow.Date,
            CreatedAtUtc = DateTime.UtcNow,
            Category = CreateCategory(categoryId, categoryName, categoryTypeId, categoryTypeName),
        };
}
