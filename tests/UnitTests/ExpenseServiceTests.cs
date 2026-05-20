using Moq;
using WebApp.Api.Services;
using WebApp.Common.Models.Expense;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace UnitTests;

public class ExpenseServiceTests
{
    private readonly Mock<IExpenseRepository> _repository = new();

    [Fact]
    public async Task GetExpensesByUserId_should_delegate_to_repository_when_no_filter()
    {
        const int userId = 3;
        var expected = new List<ExpenseResponseModel>
        {
            TestDataHelper.CreateExpense(1, 100, 1, "Groceries", 1, "Food"),
        };
        _repository.Setup(r => r.GetExpensesByUserId(userId)).ReturnsAsync(expected);

        var sut = new ExpenseService(_repository.Object);

        var result = await sut.GetExpensesByUserId(userId);

        Assert.Same(expected, result);
        _repository.Verify(r => r.GetExpensesByUserId(userId), Times.Once);
    }

    [Fact]
    public async Task GetExpensesByUserId_should_filter_by_category_date_range()
    {
        const int userId = 1;
        var expenses = new List<ExpenseResponseModel>
        {
            TestDataHelper.CreateExpense(1, 10, 1, "Groceries", 1, "Food", new DateTime(2026, 5, 10)),
            TestDataHelper.CreateExpense(2, 20, 2, "Dining Out", 1, "Food", new DateTime(2026, 5, 20)),
            TestDataHelper.CreateExpense(3, 30, 1, "Groceries", 1, "Food", new DateTime(2026, 6, 1)),
        };
        _repository.Setup(r => r.GetExpensesByUserId(userId)).ReturnsAsync(expenses);

        var filter = new ExpenseListFilter
        {
            CategoryId = 1,
            FromDate = new DateTime(2026, 5, 1),
            ToDate = new DateTime(2026, 5, 31),
        };

        var sut = new ExpenseService(_repository.Object);

        var result = await sut.GetExpensesByUserId(userId, filter);

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(10, result[0].Amount);
    }

    [Fact]
    public async Task DeleteExpenseById_should_return_true_when_repository_deletes()
    {
        const int expenseId = 5;
        _repository.Setup(r => r.DeleteAsync(expenseId)).ReturnsAsync(true);

        var sut = new ExpenseService(_repository.Object);

        var result = await sut.DeleteExpenseById(expenseId);

        Assert.True(result);
        _repository.Verify(r => r.DeleteAsync(expenseId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_should_delegate_to_repository()
    {
        var entity = new Expense
        {
            UserId = 1,
            Amount = 75,
            CategoryId = 2,
            Date = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow,
        };
        _repository.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);

        var sut = new ExpenseService(_repository.Object);

        var result = await sut.CreateAsync(entity);

        Assert.Same(entity, result);
        _repository.Verify(r => r.CreateAsync(entity), Times.Once);
    }
}
