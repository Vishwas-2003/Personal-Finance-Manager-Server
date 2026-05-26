using Moq;
using WebApp.Api.Services;
using WebApp.Common.Models.Budget;
using WebApp.Common.Models.Category;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace UnitTests;

public class BudgetServiceTests
{
    private readonly Mock<IBudgetRepository> _repository = new();

    [Fact]
    public async Task GetBudgetByUserId_should_delegate_to_repository()
    {
        var userId = 7;
        var expected = new List<BudgetResponseModel>
        {
            new()
            {
                Id = 1,
                LimitAmount = 500,
                SpentAmount = 50,
                UpdatedAtUtc = DateTime.UtcNow,
                Category = new CategoryModel
                {
                    Id = 2,
                    Name = "Groceries",
                    CategoryTypeId = 1,
                    CategoryType = "Food",
                },
            },
        };
        _repository.Setup(r => r.GetBudgetByUserId(userId)).ReturnsAsync(expected);

        var sut = new BudgetService(_repository.Object);

        var result = await sut.GetBudgetByUserId(userId);

        Assert.Equal(expected, result);
        _repository.Verify(r => r.GetBudgetByUserId(userId), Times.Once);
    }

    [Fact]
    public async Task DeleteBudgetById_should_return_true_when_repository_deletes()
    {
        const int budgetId = 42;
        _repository.Setup(r => r.DeleteAsync(budgetId)).ReturnsAsync(true);

        var sut = new BudgetService(_repository.Object);

        var result = await sut.DeleteBudgetById(budgetId);

        Assert.True(result);
        _repository.Verify(r => r.DeleteAsync(budgetId), Times.Once);
    }

    [Fact]
    public async Task DeleteBudgetById_should_return_false_when_repository_fails()
    {
        const int budgetId = 99;
        _repository.Setup(r => r.DeleteAsync(budgetId)).ReturnsAsync(false);

        var sut = new BudgetService(_repository.Object);

        var result = await sut.DeleteBudgetById(budgetId);

        Assert.False(result);
    }

    [Fact]
    public async Task CreateAsync_should_delegate_to_repository()
    {
        var entity = new Budget
        {
            UserId = 1,
            CategoryId = 2,
            LimitAmount = 200,
            SpentAmount = 0,
            UpdatedAtUtc = DateTime.UtcNow,
        };
        _repository.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);

        var sut = new BudgetService(_repository.Object);

        var result = await sut.CreateAsync(entity);

        Assert.Same(entity, result);
        _repository.Verify(r => r.CreateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task GetBudgetByUserId_should_filter_by_category_and_keywords()
    {
        const int userId = 2;
        var budgets = new List<BudgetResponseModel>
        {
            TestDataHelper.CreateBudget(1, 500, 50, 1, "Groceries", 1, "Food"),
            TestDataHelper.CreateBudget(2, 200, 0, 2, "Fuel", 2, "Transport"),
        };
        _repository.Setup(r => r.GetBudgetByUserId(userId)).ReturnsAsync(budgets);

        var sut = new BudgetService(_repository.Object);

        var result = await sut.GetBudgetByUserId(userId, new BudgetListFilter
        {
            CategoryId = 2,
            Keywords = "fuel",
        });

        Assert.Single(result);
        Assert.Equal(2, result[0].Id);
    }

    [Fact]
    public async Task UpdateBudgetById_should_update_when_budget_exists()
    {
        const int budgetId = 7;
        var existing = new Budget
        {
            Id = budgetId,
            UserId = 1,
            CategoryId = 1,
            LimitAmount = 100,
            SpentAmount = 10,
            UpdatedAtUtc = DateTime.UtcNow.AddDays(-1),
        };
        var model = new BudgetModel { UserId = 1, CategoryId = 2, LimitAmount = 300, SpentAmount = 25 };
        _repository.Setup(r => r.ReadAsync(budgetId)).ReturnsAsync(existing);
        _repository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);

        var sut = new BudgetService(_repository.Object);

        var result = await sut.UpdateBudgetById(budgetId, model);

        Assert.True(result);
        Assert.Equal(2, existing.CategoryId);
        Assert.Equal(300, existing.LimitAmount);
        Assert.Equal(25, existing.SpentAmount);
        _repository.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateBudgetById_should_return_false_when_budget_missing_or_inactive()
    {
        _repository.Setup(r => r.ReadAsync(1)).ReturnsAsync((Budget)null!);
        var sut = new BudgetService(_repository.Object);

        var missing = await sut.UpdateBudgetById(1, new BudgetModel { UserId = 1, CategoryId = 1, LimitAmount = 1, SpentAmount = 0 });
        Assert.False(missing);

        var inactive = new Budget
        {
            Id = 2,
            UserId = 1,
            CategoryId = 1,
            LimitAmount = 1,
            SpentAmount = 0,
            InActive = true,
            UpdatedAtUtc = DateTime.UtcNow,
        };
        _repository.Setup(r => r.ReadAsync(2)).ReturnsAsync(inactive);

        var inactiveResult = await sut.UpdateBudgetById(2, new BudgetModel { UserId = 1, CategoryId = 1, LimitAmount = 1, SpentAmount = 0 });
        Assert.False(inactiveResult);
    }
}
