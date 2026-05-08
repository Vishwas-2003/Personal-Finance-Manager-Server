namespace WebApp.Common.Models.Income
{
    public class IncomeModel
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; } = 0;
        public DateTime Date { get; set; }
        public required string Source { get; set; }
        public string? Notes { get; set; }
    }
}
