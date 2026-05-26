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

        Assert.Equal(expected, result);
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

    [Fact]
    public async Task CreateAsync_should_reject_future_dates()
    {
        var entity = new Expense
        {
            UserId = 1,
            Amount = 10,
            CategoryId = 1,
            Date = DateTime.Today.AddDays(2),
            CreatedAtUtc = DateTime.UtcNow,
        };
        var sut = new ExpenseService(_repository.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(entity));
        _repository.Verify(r => r.CreateAsync(It.IsAny<Expense>()), Times.Never);
    }

    [Fact]
    public async Task GetExpensesByUserId_should_filter_by_keywords()
    {
        const int userId = 1;
        var expenses = new List<ExpenseResponseModel>
        {
            TestDataHelper.CreateExpense(1, 10, 1, "Groceries", 1, "Food", description: "weekly shop"),
            TestDataHelper.CreateExpense(2, 20, 2, "Bus", 2, "Transport", description: "commute"),
        };
        _repository.Setup(r => r.GetExpensesByUserId(userId)).ReturnsAsync(expenses);

        var sut = new ExpenseService(_repository.Object);

        var result = await sut.GetExpensesByUserId(userId, new ExpenseListFilter { Keywords = "transport" });

        Assert.Single(result);
        Assert.Equal(2, result[0].Id);
    }

    [Fact]
    public async Task UpdateExpenseById_should_update_when_expense_exists()
    {
        const int expenseId = 3;
        var existing = new Expense
        {
            Id = expenseId,
            UserId = 1,
            Amount = 10,
            CategoryId = 1,
            Date = DateTime.Today,
            CreatedAtUtc = DateTime.UtcNow,
        };
        var model = new ExpenseModel
        {
            UserId = 1,
            Amount = 25,
            CategoryId = 2,
            Description = "Updated",
            Date = DateTime.Today,
        };
        _repository.Setup(r => r.ReadAsync(expenseId)).ReturnsAsync(existing);
        _repository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);

        var sut = new ExpenseService(_repository.Object);

        var result = await sut.UpdateExpenseById(expenseId, model);

        Assert.True(result);
        Assert.Equal(25, existing.Amount);
        Assert.Equal(2, existing.CategoryId);
        Assert.Equal("Updated", existing.Description);
        _repository.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateExpenseById_should_return_false_when_expense_missing()
    {
        _repository.Setup(r => r.ReadAsync(99)).ReturnsAsync((Expense)null!);
        var sut = new ExpenseService(_repository.Object);

        var result = await sut.UpdateExpenseById(99, new ExpenseModel
        {
            UserId = 1,
            Amount = 1,
            CategoryId = 1,
            Date = DateTime.Today,
        });

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateExpenseById_should_return_false_when_expense_is_inactive()
    {
        var existing = new Expense
        {
            Id = 4,
            UserId = 1,
            Amount = 10,
            CategoryId = 1,
            Date = DateTime.Today,
            InActive = true,
            CreatedAtUtc = DateTime.UtcNow,
        };
        _repository.Setup(r => r.ReadAsync(4)).ReturnsAsync(existing);
        var sut = new ExpenseService(_repository.Object);

        var result = await sut.UpdateExpenseById(4, new ExpenseModel
        {
            UserId = 1,
            Amount = 1,
            CategoryId = 1,
            Date = DateTime.Today,
        });

        Assert.False(result);
    }
}
