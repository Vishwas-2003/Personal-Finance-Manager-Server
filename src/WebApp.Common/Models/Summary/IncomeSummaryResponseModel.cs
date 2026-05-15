using WebApp.Common.Models.Income;

namespace WebApp.Common.Models.Summary
{
    public class IncomeSummaryResponseModel
    {
        public List<IncomeResponseModel> Incomes { get; set; } = new List<IncomeResponseModel>();
        public decimal TotalIncome { get; set; }
    }
}
