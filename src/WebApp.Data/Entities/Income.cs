namespace WebApp.Data.Entities;

public class Income
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public required string Source { get; set; }
    public string? Notes { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public bool InActive { get; set; }
}
