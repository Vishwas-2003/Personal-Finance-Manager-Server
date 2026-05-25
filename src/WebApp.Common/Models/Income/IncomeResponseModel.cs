using WebApp.Common.Models.Category;

namespace WebApp.Common.Models.Income
{
    public class IncomeResponseModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public required string Source { get; set; }
        public string? Notes { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public bool InActive { get; set; }
        public CategoryModel Category { get; set; } = null!;
    }
}
