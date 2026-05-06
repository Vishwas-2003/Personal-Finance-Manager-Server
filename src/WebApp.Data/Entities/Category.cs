using WebApp.Common.Enums;

namespace WebApp.Data.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public CategoryType Type { get; set; }
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
