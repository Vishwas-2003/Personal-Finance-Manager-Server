using WebApp.Common.Models.Category;
using WebApp.Common.Models.User;

namespace WebApp.Common.Models.Budget
{
    public class BudgetResponseModel
    {
        public int Id { get; set; }
        public decimal LimitAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public CategoryModel Category { get; set; } = null!;
    }
}
