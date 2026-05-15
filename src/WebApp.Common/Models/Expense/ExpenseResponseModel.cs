using WebApp.Common.Models.Category;

namespace WebApp.Common.Models.Expense
{
    public class ExpenseResponseModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public CategoryModel Category { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
