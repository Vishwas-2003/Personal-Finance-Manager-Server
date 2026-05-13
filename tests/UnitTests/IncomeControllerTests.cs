using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Api.Controllers;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Income;
using WebApp.Data.Entities;

namespace UnitTests;

public class IncomeControllerTests
{
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IIncomeService> _incomeService = new();

    [Fact]
    public async Task AddIncome_returns_Ok_when_create_returns_entity()
    {
        var model = new IncomeModel
        {
            UserId = 1,
            Amount = 100,
            CategoryId = 2,
            Date = DateTime.UtcNow,
            Source = "Bonus",
        };
        var entity = new Income
        {
            UserId = 1,
            Amount = 100,
            CategoryId = 2,
            Source = "Bonus",
            Date = model.Date,
            CreatedAtUtc = DateTime.UtcNow,
        };
        _mapper.Setup(m => m.Map<Income>(model)).Returns(entity);
        _incomeService.Setup(s => s.CreateAsync(entity)).ReturnsAsync(entity);
        var sut = new IncomeController(_mapper.Object, _incomeService.Object);

        var result = await sut.AddIncome(model);

        Assert.IsType<OkResult>(result);
        _incomeService.Verify(s => s.CreateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task AddIncome_returns_BadRequest_when_create_returns_null()
    {
        var model = new IncomeModel
        {
            UserId = 1,
            Amount = 50,
            CategoryId = 2,
            Date = DateTime.UtcNow,
            Source = "Gift",
        };
        var entity = new Income
        {
            UserId = 1,
            Amount = 50,
            CategoryId = 2,
            Source = "Gift",
            Date = model.Date,
            CreatedAtUtc = DateTime.UtcNow,
        };
        _mapper.Setup(m => m.Map<Income>(model)).Returns(entity);
        _incomeService.Setup(s => s.CreateAsync(entity)).ReturnsAsync((Income)null!);
        var sut = new IncomeController(_mapper.Object, _incomeService.Object);

        var result = await sut.AddIncome(model);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task GetIncome_returns_Ok_with_payload_when_list_is_returned()
    {
        var list = new List<IncomeResponseModel>();
        _incomeService.Setup(s => s.GetIncomeByUserId(2)).ReturnsAsync(list);
        var sut = new IncomeController(_mapper.Object, _incomeService.Object);

        var result = await sut.GetIncome(2);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(list, ok.Value);
    }

    [Fact]
    public async Task GetIncome_returns_BadRequest_when_service_returns_null()
    {
        _incomeService.Setup(s => s.GetIncomeByUserId(2)).ReturnsAsync((List<IncomeResponseModel>)null!);
        var sut = new IncomeController(_mapper.Object, _incomeService.Object);

        var result = await sut.GetIncome(2);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task DeleteIncome_returns_Ok_when_delete_succeeds()
    {
        _incomeService.Setup(s => s.DeleteIncomeById(11)).ReturnsAsync(true);
        var sut = new IncomeController(_mapper.Object, _incomeService.Object);

        var result = await sut.DeleteIncome(11);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteIncome_returns_BadRequest_with_message_when_delete_fails()
    {
        _incomeService.Setup(s => s.DeleteIncomeById(11)).ReturnsAsync(false);
        var sut = new IncomeController(_mapper.Object, _incomeService.Object);

        var result = await sut.DeleteIncome(11);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Delete Failed!", badRequest.Value);
    }
}
