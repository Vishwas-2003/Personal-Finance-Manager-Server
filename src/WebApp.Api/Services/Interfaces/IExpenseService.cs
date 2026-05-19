using WebApp.Common.Models.Expense;
using WebApp.Data.Entities;

namespace WebApp.Api.Services.Interfaces
{
    public interface IExpenseService : ICRUDBaseService<Expense>
    {
        Task<bool> DeleteExpenseById(int expenseId);
        Task<List<ExpenseResponseModel>> GetExpensesByUserId(int userId, ExpenseListFilter? filter = null);
    }
}
