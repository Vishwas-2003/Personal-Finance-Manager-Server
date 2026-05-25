namespace WebApp.Data.Entities;

public class Budget
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public decimal LimitAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public bool InActive { get; set; }
}
