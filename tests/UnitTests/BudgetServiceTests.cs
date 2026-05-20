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

        Assert.Same(expected, result);
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
}
