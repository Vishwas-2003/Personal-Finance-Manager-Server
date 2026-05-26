using Moq;
using WebApp.Api.Services;
using WebApp.Common.Models.Category;
using WebApp.Common.Models.Income;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace UnitTests;

public class IncomeServiceTests
{
    private readonly Mock<IIncomeRepository> _repository = new();

    [Fact]
    public async Task GetIncomeByUserId_should_delegate_to_repository()
    {
        var userId = 3;
        var expected = new List<IncomeResponseModel>
        {
            new()
            {
                Id = 10,
                Amount = 1000,
                Source = "Salary",
                Notes = null,
                Date = DateTime.UtcNow.Date,
                CreatedAtUtc = DateTime.UtcNow,
                Category = new CategoryModel
                {
                    Id = 7,
                    Name = "Monthly Salary",
                    CategoryTypeId = 4,
                    CategoryType = "Salary",
                },
            },
        };
        _repository.Setup(r => r.GetIncomeByUserId(userId)).ReturnsAsync(expected);

        var sut = new IncomeService(_repository.Object);

        var result = await sut.GetIncomeByUserId(userId);

        Assert.Equal(expected, result);
        _repository.Verify(r => r.GetIncomeByUserId(userId), Times.Once);
    }

    [Fact]
    public async Task DeleteIncomeById_should_return_true_when_repository_deletes()
    {
        const int incomeId = 5;
        _repository.Setup(r => r.DeleteAsync(incomeId)).ReturnsAsync(true);

        var sut = new IncomeService(_repository.Object);

        var result = await sut.DeleteIncomeById(incomeId);

        Assert.True(result);
        _repository.Verify(r => r.DeleteAsync(incomeId), Times.Once);
    }

    [Fact]
    public async Task DeleteIncomeById_should_return_false_when_repository_fails()
    {
        const int incomeId = 8;
        _repository.Setup(r => r.DeleteAsync(incomeId)).ReturnsAsync(false);

        var sut = new IncomeService(_repository.Object);

        var result = await sut.DeleteIncomeById(incomeId);

        Assert.False(result);
    }

    [Fact]
    public async Task CreateAsync_should_delegate_to_repository()
    {
        var entity = new Income
        {
            UserId = 1,
            Amount = 500,
            CategoryId = 2,
            Source = "Freelance",
            Date = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow,
        };
        _repository.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);

        var sut = new IncomeService(_repository.Object);

        var result = await sut.CreateAsync(entity);

        Assert.Same(entity, result);
        _repository.Verify(r => r.CreateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_should_reject_future_dates()
    {
        var entity = new Income
        {
            UserId = 1,
            Amount = 100,
            CategoryId = 1,
            Source = "Bonus",
            Date = DateTime.Today.AddDays(3),
            CreatedAtUtc = DateTime.UtcNow,
        };
        var sut = new IncomeService(_repository.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(entity));
    }

    [Fact]
    public async Task GetIncomeByUserId_should_apply_category_date_and_keyword_filters()
    {
        const int userId = 5;
        var incomes = new List<IncomeResponseModel>
        {
            TestDataHelper.CreateIncome(1, 100, 1, "Salary", 4, "Income", new DateTime(2026, 5, 5), "payroll"),
            TestDataHelper.CreateIncome(2, 200, 2, "Freelance", 4, "Income", new DateTime(2026, 6, 1), "client work"),
            TestDataHelper.CreateIncome(3, 50, 1, "Salary", 4, "Income", new DateTime(2026, 5, 20), "bonus"),
        };
        _repository.Setup(r => r.GetIncomeByUserId(userId)).ReturnsAsync(incomes);

        var filter = new IncomeListFilter
        {
            CategoryId = 1,
            FromDate = new DateTime(2026, 5, 1),
            ToDate = new DateTime(2026, 5, 31),
            Keywords = "bonus",
        };
        var sut = new IncomeService(_repository.Object);

        var result = await sut.GetIncomeByUserId(userId, filter);

        Assert.Single(result);
        Assert.Equal(3, result[0].Id);
    }

    [Fact]
    public async Task UpdateIncomeById_should_update_when_income_exists()
    {
        const int incomeId = 11;
        var existing = new Income
        {
            Id = incomeId,
            UserId = 1,
            Amount = 100,
            CategoryId = 1,
            Source = "Old",
            Date = DateTime.Today,
            CreatedAtUtc = DateTime.UtcNow,
        };
        var model = new IncomeModel
        {
            UserId = 1,
            Amount = 150,
            CategoryId = 2,
            Source = "New",
            Notes = "note",
            Date = DateTime.Today,
        };
        _repository.Setup(r => r.ReadAsync(incomeId)).ReturnsAsync(existing);
        _repository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);

        var sut = new IncomeService(_repository.Object);

        var result = await sut.UpdateIncomeById(incomeId, model);

        Assert.True(result);
        Assert.Equal(150, existing.Amount);
        Assert.Equal(2, existing.CategoryId);
        Assert.Equal("New", existing.Source);
        Assert.Equal("note", existing.Notes);
    }

    [Fact]
    public async Task UpdateIncomeById_should_return_false_when_income_missing_or_inactive()
    {
        _repository.Setup(r => r.ReadAsync(1)).ReturnsAsync((Income)null!);
        var sut = new IncomeService(_repository.Object);

        var missing = await sut.UpdateIncomeById(1, new IncomeModel
        {
            UserId = 1,
            Amount = 1,
            CategoryId = 1,
            Date = DateTime.Today,
            Source = "x",
        });
        Assert.False(missing);

        var inactive = new Income
        {
            Id = 2,
            UserId = 1,
            Amount = 1,
            CategoryId = 1,
            Source = "x",
            Date = DateTime.Today,
            InActive = true,
            CreatedAtUtc = DateTime.UtcNow,
        };
        _repository.Setup(r => r.ReadAsync(2)).ReturnsAsync(inactive);

        var inactiveResult = await sut.UpdateIncomeById(2, new IncomeModel
        {
            UserId = 1,
            Amount = 1,
            CategoryId = 1,
            Date = DateTime.Today,
            Source = "x",
        });
        Assert.False(inactiveResult);
    }
}
