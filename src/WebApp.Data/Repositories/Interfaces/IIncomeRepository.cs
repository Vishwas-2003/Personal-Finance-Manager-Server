using WebApp.Common.Models.Income;
using WebApp.Data.Entities;

namespace WebApp.Data.Repositories.Interfaces
{
    public interface IIncomeRepository : ICRUDBaseRepository<Income>
    {
        Task<List<IncomeResponseModel>> GetIncomeByUserId(int userId);
    }
}
