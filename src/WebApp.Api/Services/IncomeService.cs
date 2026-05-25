using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Income;
using WebApp.Common.Utilities;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services;

public class IncomeService : CRUDBaseService<Income>, IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;

    public IncomeService(IIncomeRepository incomeRepository) : base(incomeRepository)
    {
        _incomeRepository = incomeRepository;
    }

    public override async Task<Income> CreateAsync(Income entity)
    {
        TransactionDateValidator.EnsureNotFuture(entity.Date);
        return await base.CreateAsync(entity);
    }

    public async Task<List<IncomeResponseModel>> GetIncomeByUserId(int userId, IncomeListFilter? filter = null)
    {
        var incomes = await _incomeRepository.GetIncomeByUserId(userId);

        if (filter?.CategoryId is int categoryId)
        {
            incomes = incomes.Where(i => i.Category.Id == categoryId).ToList();
        }

        if (filter?.FromDate is DateTime fromDate)
        {
            incomes = incomes.Where(i => i.Date.Date >= fromDate.Date).ToList();
        }

        if (filter?.ToDate is DateTime toDate)
        {
            incomes = incomes.Where(i => i.Date.Date <= toDate.Date).ToList();
        }

        var keywords = KeywordFilterUtility.SplitKeywords(filter?.Keywords).ToList();
        if (keywords.Count > 0)
        {
            incomes = incomes.Where(i => KeywordFilterUtility.MatchesAnyKeyword(
                [
                    i.Id.ToString(),
                    i.Amount.ToString("0.##"),
                    i.Category.Name,
                    i.Category.CategoryType,
                    i.Source,
                    i.Notes ?? string.Empty,
                    i.Date.ToString("yyyy-MM-dd"),
                    i.InActive ? "archived" : "active"
                ],
                keywords)).ToList();
        }

        return incomes
            .OrderByDescending(i => i.Date)
            .ThenByDescending(i => i.Id)
            .ToList();
    }

    public async Task<bool> DeleteIncomeById(int incomeId) => await DeleteAsync(incomeId);

    public async Task<bool> UpdateIncomeById(int incomeId, IncomeModel model)
    {
        TransactionDateValidator.EnsureNotFuture(model.Date);

        var income = await ReadAsync(incomeId);
        if (income is null || income.InActive)
        {
            return false;
        }

        income.Amount = model.Amount;
        income.CategoryId = model.CategoryId;
        income.Date = model.Date;
        income.Source = model.Source;
        income.Notes = model.Notes;
        await UpdateAsync(income);
        return true;
    }
}
