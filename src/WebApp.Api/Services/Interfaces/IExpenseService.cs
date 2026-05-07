using WebApp.Common.Models.Expense;
using WebApp.Data.Entities;

namespace WebApp.Api.Services.Interfaces
{
    public interface IExpenseService : ICRUDBaseService<Expense>
    {
        Task<bool> DeleteExpenseById(int expenseId);
        Task<List<ExpenceResponseModel>> GetExpensesByUserId(int userId);
    }
}
