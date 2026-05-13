namespace WebApp.Common.Models.Budget
{
    public class BudgetModel
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal LimitAmount { get; set; }
        public decimal SpentAmount { get; set; }
    }
}
