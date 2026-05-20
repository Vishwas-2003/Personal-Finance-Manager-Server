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

        Assert.Same(expected, result);
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
}
