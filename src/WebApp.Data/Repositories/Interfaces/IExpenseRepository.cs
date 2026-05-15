using WebApp.Common.Models.Expense;
using WebApp.Data.Entities;

namespace WebApp.Data.Repositories.Interfaces
{
    public interface IExpenseRepository : ICRUDBaseRepository<Expense>
    {
        Task<List<ExpenseResponseModel>> GetExpensesByUserId(int userId);
    }
}
