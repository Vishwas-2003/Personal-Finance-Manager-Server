using WebApp.Common.Models.Expense;
using WebApp.Data.Entities;

namespace WebApp.Data.Repositories.Interfaces
{
    public interface IExpenseRepository : ICRUDBaseRepository<Expense>
    {
        Task<List<ExpenceResponseModel>> GetExpensesByUserId(int userId);
    }
}
