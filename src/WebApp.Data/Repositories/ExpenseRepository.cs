using WebApp.Data.Entities;
using WebApp.Data.Persistence;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Data.Repositories
{
    public class ExpenseRepository : CRUDBaseRepository<Expense>, IExpenseRepository
    {
        private readonly AppDbContext _dbContext;
        public ExpenseRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
