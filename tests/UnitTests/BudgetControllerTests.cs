using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Api.Controllers;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Budget;
using WebApp.Data.Entities;

namespace UnitTests;

public class BudgetControllerTests
{
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IBudgetService> _budgetService = new();

    [Fact]
    public async Task AddBudget_returns_Ok_when_create_returns_entity()
    {
        var model = new BudgetModel { UserId = 1, CategoryId = 2, LimitAmount = 300, SpentAmount = 0 };
        var entity = new Budget
        {
            Id = 0,
            UserId = 1,
            CategoryId = 2,
            LimitAmount = 300,
            SpentAmount = 0,
            UpdatedAtUtc = DateTime.UtcNow,
        };
        _mapper.Setup(m => m.Map<Budget>(model)).Returns(entity);
        _budgetService.Setup(s => s.CreateAsync(entity)).ReturnsAsync(entity);
        var sut = new BudgetController(_mapper.Object, _budgetService.Object);

        var result = await sut.AddBudget(model);

        Assert.IsType<OkResult>(result);
        _budgetService.Verify(s => s.CreateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task AddBudget_returns_BadRequest_when_create_returns_null()
    {
        var model = new BudgetModel { UserId = 1, CategoryId = 2, LimitAmount = 100, SpentAmount = 0 };
        var entity = new Budget
        {
            UserId = 1,
            CategoryId = 2,
            LimitAmount = 100,
            SpentAmount = 0,
            UpdatedAtUtc = DateTime.UtcNow,
        };
        _mapper.Setup(m => m.Map<Budget>(model)).Returns(entity);
        _budgetService.Setup(s => s.CreateAsync(entity)).ReturnsAsync((Budget)null!);
        var sut = new BudgetController(_mapper.Object, _budgetService.Object);

        var result = await sut.AddBudget(model);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task GetBudget_returns_Ok_with_payload_when_list_is_returned()
    {
        var list = new List<BudgetResponseModel>();
        _budgetService.Setup(s => s.GetBudgetByUserId(4)).ReturnsAsync(list);
        var sut = new BudgetController(_mapper.Object, _budgetService.Object);

        var result = await sut.GetBudget(4, null);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(list, ok.Value);
    }

    [Fact]
    public async Task GetBudget_returns_BadRequest_when_service_returns_null()
    {
        _budgetService.Setup(s => s.GetBudgetByUserId(4)).ReturnsAsync((List<BudgetResponseModel>)null!);
        var sut = new BudgetController(_mapper.Object, _budgetService.Object);

        var result = await sut.GetBudget(4, null);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task DeleteBudget_returns_Ok_when_delete_succeeds()
    {
        _budgetService.Setup(s => s.DeleteBudgetById(9)).ReturnsAsync(true);
        var sut = new BudgetController(_mapper.Object, _budgetService.Object);

        var result = await sut.DeleteBudget(9);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteBudget_returns_BadRequest_with_message_when_delete_fails()
    {
        _budgetService.Setup(s => s.DeleteBudgetById(9)).ReturnsAsync(false);
        var sut = new BudgetController(_mapper.Object, _budgetService.Object);

        var result = await sut.DeleteBudget(9);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Delete Failed!", badRequest.Value);
    }

    [Fact]
    public async Task UpdateBudget_returns_Ok_when_update_succeeds()
    {
        var model = new BudgetModel { UserId = 1, CategoryId = 2, LimitAmount = 400, SpentAmount = 10 };
        _budgetService.Setup(s => s.UpdateBudgetById(6, model)).ReturnsAsync(true);
        var sut = new BudgetController(_mapper.Object, _budgetService.Object);

        var result = await sut.UpdateBudget(6, model);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateBudget_returns_BadRequest_when_update_fails()
    {
        var model = new BudgetModel { UserId = 1, CategoryId = 2, LimitAmount = 400, SpentAmount = 10 };
        _budgetService.Setup(s => s.UpdateBudgetById(6, model)).ReturnsAsync(false);
        var sut = new BudgetController(_mapper.Object, _budgetService.Object);

        var result = await sut.UpdateBudget(6, model);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Update failed!", badRequest.Value);
    }
}
