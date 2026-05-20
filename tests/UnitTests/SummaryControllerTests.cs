using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Api.Controllers;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Summary;

namespace UnitTests;

public class SummaryControllerTests
{
    private readonly Mock<ISummaryService> _summaryService = new();

    [Fact]
    public async Task GetTotalIncomeSummary_returns_Ok_with_payload()
    {
        var summary = new IncomeSummaryResponseModel { TotalIncome = 500 };
        _summaryService.Setup(s => s.GetTotalIncomeSummary(1)).ReturnsAsync(summary);
        var sut = new SummaryController(_summaryService.Object);

        var result = await sut.GetTotalIncomeSummary(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(summary, ok.Value);
    }

    [Fact]
    public async Task GetTotalIncomeSummary_returns_BadRequest_when_service_returns_null()
    {
        _summaryService.Setup(s => s.GetTotalIncomeSummary(1)).ReturnsAsync((IncomeSummaryResponseModel)null!);
        var sut = new SummaryController(_summaryService.Object);

        var result = await sut.GetTotalIncomeSummary(1);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task GetTotalExpenseSummary_returns_Ok_with_payload()
    {
        var summary = new ExpenseSummaryResponseModel { TotalExpense = 200 };
        _summaryService.Setup(s => s.GetTotalExpenseSummary(2)).ReturnsAsync(summary);
        var sut = new SummaryController(_summaryService.Object);

        var result = await sut.GetTotalExpenseSummary(2);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(summary, ok.Value);
    }

    [Fact]
    public async Task GetBalanceSummary_returns_Ok_and_passes_filter()
    {
        var summary = new BalanceSummaryResponseModel { Balance = 100 };
        var filter = new BalanceSummaryFilter { FromDate = new DateTime(2026, 5, 1) };
        _summaryService.Setup(s => s.GetBalanceSummary(3, filter)).ReturnsAsync(summary);
        var sut = new SummaryController(_summaryService.Object);

        var result = await sut.GetBalanceSummary(3, filter);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(summary, ok.Value);
        _summaryService.Verify(s => s.GetBalanceSummary(3, filter), Times.Once);
    }
}
