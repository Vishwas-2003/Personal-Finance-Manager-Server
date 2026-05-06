namespace WebApp.Common.Models.Expense
{
    public class ExpenseModel
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }
}
