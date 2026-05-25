using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Models.Income;
using WebApp.Data.Entities;
using WebApp.Data.Persistence;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Data.Repositories;

public sealed class IncomeRepository : CRUDBaseRepository<Income>, IIncomeRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public IncomeRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<IncomeResponseModel>> GetIncomeByUserId(int userId)
    {
        var incomes = await _dbContext.Incomes
            .Include(income => income.Category)
                .ThenInclude(category => category.CategoryType)
            .Where(e => e.UserId == userId)
            .ToListAsync();

        return _mapper.Map<List<IncomeResponseModel>>(incomes);
    }

    public override async Task<bool> DeleteAsync(int id)
    {
        var entity = await ReadAsync(id);
        if (entity is null)
        {
            return false;
        }

        entity.InActive = true;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
