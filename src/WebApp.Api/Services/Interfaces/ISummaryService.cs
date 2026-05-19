using WebApp.Common.Models.Summary;

namespace WebApp.Api.Services.Interfaces
{
    public interface ISummaryService
    {
        Task<IncomeSummaryResponseModel> GetTotalIncomeSummary(int userId);
        Task<ExpenseSummaryResponseModel> GetTotalExpenseSummary(int userId);
        Task<BalanceSummaryResponseModel> GetBalanceSummary(int userId, BalanceSummaryFilter? filter = null);
    }
}
