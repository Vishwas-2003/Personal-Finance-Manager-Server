using WebApp.Common.Models.Income;
using WebApp.Data.Entities;

namespace WebApp.Api.Services.Interfaces
{
    public interface IIncomeService : ICRUDBaseService<Income>
    {
        Task<bool> DeleteIncomeById(int incomeId);
        Task<List<IncomeResponseModel>> GetIncomeByUserId(int userId);
    }
}
