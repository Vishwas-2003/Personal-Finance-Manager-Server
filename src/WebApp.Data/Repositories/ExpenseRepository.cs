using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Models.Expense;
using WebApp.Data.Entities;
using WebApp.Data.Persistence;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Data.Repositories;

public sealed class ExpenseRepository : CRUDBaseRepository<Expense>, IExpenseRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ExpenseRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<ExpenseResponseModel>> GetExpensesByUserId(int userId)
    {
        var expenses = await _dbContext.Expenses
            .Include(expense => expense.Category)
                .ThenInclude(category => category.CategoryType)
            .Where(e => e.UserId == userId)
            .ToListAsync();

        return _mapper.Map<List<ExpenseResponseModel>>(expenses);
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
