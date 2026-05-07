using WebApp.Common.Enums;
using WebApp.Common.Models.Category;
using WebApp.Common.Models.User;

namespace WebApp.Common.Models.Expense
{
    public class ExpenceResponseModel
    {
        public int Id { get; set; }
        public UserResponseModel User { get; set; } = null!;
        public decimal Amount { get; set; }
        public CategoryModel Category { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
