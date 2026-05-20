using Moq;
using WebApp.Api.Services;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Expense;
using WebApp.Common.Models.Income;
using WebApp.Common.Models.Summary;
using WebApp.Data.Repositories.Interfaces;

namespace UnitTests;

public class SummaryServiceTests
{
    private readonly Mock<IIncomeRepository> _incomeRepository = new();
    private readonly Mock<IExpenseRepository> _expenseRepository = new();
    private readonly Mock<IIncomeService> _incomeService = new();
    private readonly Mock<IExpenseService> _expenseService = new();

    [Fact]
    public async Task GetTotalIncomeSummary_should_aggregate_by_category_hierarchy()
    {
        const int userId = 1;
        var incomes = new List<IncomeResponseModel>
        {
            TestDataHelper.CreateIncome(1, 100, 1, "Groceries", 1, "Food"),
            TestDataHelper.CreateIncome(2, 200, 7, "Monthly Salary", 4, "Salary"),
        };
        _incomeRepository.Setup(r => r.GetIncomeByUserId(userId)).ReturnsAsync(incomes);

        var sut = CreateSut();

        var result = await sut.GetTotalIncomeSummary(userId);

        Assert.Equal(300, result.TotalIncome);
        Assert.Equal(2, result.CategoryTypeSections.Count);
    }

    [Fact]
    public async Task GetTotalExpenseSummary_should_aggregate_by_category_hierarchy()
    {
        const int userId = 2;
        var expenses = new List<ExpenseResponseModel>
        {
            TestDataHelper.CreateExpense(1, 50, 1, "Groceries", 1, "Food"),
            TestDataHelper.CreateExpense(2, 25, 2, "Fuel", 2, "Transport"),
        };
        _expenseRepository.Setup(r => r.GetExpensesByUserId(userId)).ReturnsAsync(expenses);

        var sut = CreateSut();

        var result = await sut.GetTotalExpenseSummary(userId);

        Assert.Equal(75, result.TotalExpense);
        Assert.Equal(2, result.CategoryTypeSections.Count);
    }

    [Fact]
    public async Task GetBalanceSummary_should_compute_credit_debit_and_balance()
    {
        const int userId = 3;
        var incomes = new List<IncomeResponseModel>
        {
            TestDataHelper.CreateIncome(1, 1000, 7, "Salary", 4, "Salary"),
            TestDataHelper.CreateIncome(2, 100, 8, "Bonus", 4, "Salary"),
        };
        var expenses = new List<ExpenseResponseModel>
        {
            TestDataHelper.CreateExpense(1, 250, 1, "Groceries", 1, "Food"),
        };
        _incomeService.Setup(s => s.GetIncomeByUserId(userId)).ReturnsAsync(incomes);
        _expenseService.Setup(s => s.GetExpensesByUserId(userId, null)).ReturnsAsync(expenses);

        var sut = CreateSut();

        var result = await sut.GetBalanceSummary(userId);

        Assert.Equal(1100, result.TotalCredit);
        Assert.Equal(250, result.TotalDebit);
        Assert.Equal(850, result.Balance);
        Assert.Equal(2, result.Credits.Count);
        Assert.Single(result.Debits);
    }

    [Fact]
    public async Task GetBalanceSummary_should_apply_date_filter_to_income_and_expenses()
    {
        const int userId = 4;
        var from = new DateTime(2026, 5, 1);
        var to = new DateTime(2026, 5, 31);
        var filter = new BalanceSummaryFilter { FromDate = from, ToDate = to };
        var incomes = new List<IncomeResponseModel>
        {
            TestDataHelper.CreateIncome(1, 500, 7, "Salary", 4, "Salary", new DateTime(2026, 5, 15)),
            TestDataHelper.CreateIncome(2, 300, 8, "Bonus", 4, "Salary", new DateTime(2026, 6, 1)),
        };
        var expenses = new List<ExpenseResponseModel>
        {
            TestDataHelper.CreateExpense(1, 100, 1, "Groceries", 1, "Food", new DateTime(2026, 5, 10)),
        };
        _incomeService.Setup(s => s.GetIncomeByUserId(userId)).ReturnsAsync(incomes);
        _expenseService.Setup(s => s.GetExpensesByUserId(
                userId,
                It.Is<ExpenseListFilter>(f => f.FromDate == from && f.ToDate == to)))
            .ReturnsAsync(expenses);

        var sut = CreateSut();

        var result = await sut.GetBalanceSummary(userId, filter);

        Assert.Equal(500, result.TotalCredit);
        Assert.Equal(100, result.TotalDebit);
        Assert.Equal(400, result.Balance);
        Assert.Single(result.Credits);
    }

    private SummaryService CreateSut() =>
        new(
            _incomeRepository.Object,
            _expenseRepository.Object,
            _incomeService.Object,
            _expenseService.Object);
}
