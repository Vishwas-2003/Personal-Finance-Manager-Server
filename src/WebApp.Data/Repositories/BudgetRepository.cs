using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Models.Budget;
using WebApp.Data.Entities;
using WebApp.Data.Persistence;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Data.Repositories
{
    public class BudgetRepository : CRUDBaseRepository<Budget>, IBudgetRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public BudgetRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<BudgetResponseModel>> GetBudgetByUserId(int userId)
        {
            var budgets = await _dbContext.Budgets
                .Include(budget => budget.Category)
                    .ThenInclude(category => category.CategoryType)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<BudgetResponseModel>>(budgets);
        }
    }
}
