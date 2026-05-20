using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Api.Controllers;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Expense;
using WebApp.Data.Entities;

namespace UnitTests;

public class ExpenseControllerTests
{
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IExpenseService> _expenseService = new();

    [Fact]
    public async Task AddExpense_returns_Ok_when_create_returns_entity()
    {
        var model = new ExpenseModel
        {
            UserId = 1,
            Amount = 100,
            CategoryId = 2,
            Description = "Lunch",
            Date = DateTime.UtcNow,
        };
        var entity = new Expense
        {
            UserId = 1,
            Amount = 100,
            CategoryId = 2,
            Description = "Lunch",
            Date = model.Date,
            CreatedAtUtc = DateTime.UtcNow,
        };
        _mapper.Setup(m => m.Map<Expense>(model)).Returns(entity);
        _expenseService.Setup(s => s.CreateAsync(entity)).ReturnsAsync(entity);
        var sut = new ExpenseController(_expenseService.Object, _mapper.Object);

        var result = await sut.AddExpense(model);

        Assert.IsType<OkResult>(result);
        _expenseService.Verify(s => s.CreateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task GetExpenses_returns_Ok_and_passes_filter_to_service()
    {
        var list = new List<ExpenseResponseModel>();
        var filter = new ExpenseListFilter { CategoryId = 2 };
        _expenseService.Setup(s => s.GetExpensesByUserId(2, filter)).ReturnsAsync(list);
        var sut = new ExpenseController(_expenseService.Object, _mapper.Object);

        var result = await sut.GetExpenses(2, filter);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(list, ok.Value);
        _expenseService.Verify(s => s.GetExpensesByUserId(2, filter), Times.Once);
    }

    [Fact]
    public async Task GetExpenses_returns_BadRequest_when_service_returns_null()
    {
        _expenseService.Setup(s => s.GetExpensesByUserId(2, null)).ReturnsAsync((List<ExpenseResponseModel>)null!);
        var sut = new ExpenseController(_expenseService.Object, _mapper.Object);

        var result = await sut.GetExpenses(2, null);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task DeleteExpense_returns_Ok_when_delete_succeeds()
    {
        _expenseService.Setup(s => s.DeleteExpenseById(11)).ReturnsAsync(true);
        var sut = new ExpenseController(_expenseService.Object, _mapper.Object);

        var result = await sut.DeleteExpense(11);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteExpense_returns_BadRequest_with_message_when_delete_fails()
    {
        _expenseService.Setup(s => s.DeleteExpenseById(11)).ReturnsAsync(false);
        var sut = new ExpenseController(_expenseService.Object, _mapper.Object);

        var result = await sut.DeleteExpense(11);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Delete Failed!", badRequest.Value);
    }
}
