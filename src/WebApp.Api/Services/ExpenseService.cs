using WebApp.Api.Services.Interfaces;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services
{
    public class ExpenseService : CRUDBaseService<Expense>, IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        public ExpenseService(IExpenseRepository expenseRepository) : base(expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }
    }
}
