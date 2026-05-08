using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Income;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services
{
    public class IncomeService : CRUDBaseService<Income>, IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        public IncomeService(IIncomeRepository incomeRepository) : base(incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public async Task<List<IncomeResponseModel>> GetIncomeByUserId(int userId)
        {
            return await _incomeRepository.GetIncomeByUserId(userId);
        }

        public async Task<bool> DeleteIncomeById(int incomeId)
        {
            return await DeleteAsync(incomeId);
        }
    }
}
