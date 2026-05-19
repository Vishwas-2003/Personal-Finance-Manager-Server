using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Expense;
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

        public async Task<List<ExpenseResponseModel>> GetExpensesByUserId(int userId, ExpenseListFilter? filter = null)
        {
            var expenses = await _expenseRepository.GetExpensesByUserId(userId);

            if (filter?.CategoryId is int categoryId)
            {
                expenses = expenses.Where(e => e.Category.Id == categoryId).ToList();
            }

            if (filter?.FromDate is DateTime fromDate)
            {
                expenses = expenses.Where(e => e.Date.Date >= fromDate.Date).ToList();
            }

            if (filter?.ToDate is DateTime toDate)
            {
                expenses = expenses.Where(e => e.Date.Date <= toDate.Date).ToList();
            }

            return expenses;
        }

        public async  Task<bool> DeleteExpenseById(int expenseId)
        {
            return await DeleteAsync(expenseId);
        }
    }
}
