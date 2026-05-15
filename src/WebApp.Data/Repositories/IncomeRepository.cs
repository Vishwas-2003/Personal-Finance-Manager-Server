using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Models.Income;
using WebApp.Data.Entities;
using WebApp.Data.Persistence;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Data.Repositories
{
    public class IncomeRepository : CRUDBaseRepository<Income>, IIncomeRepository
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
            var Incomes = await _dbContext.Incomes
                .Include(income => income.Category)
                    .ThenInclude(category => category.CategoryType)
                .Where(e => e.UserId == userId).ToListAsync();

            return _mapper.Map<List<IncomeResponseModel>>(Incomes);
        }
    }
}
