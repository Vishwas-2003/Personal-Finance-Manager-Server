using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Data.Entities;

namespace WebApp.Data.Persistence.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable(
            "Budgets",
            t => t.HasCheckConstraint("CK_Budgets_LimitAmount", "[LimitAmount] > 0"));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.LimitAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.SpentAmount)
            .HasPrecision(18, 2)
            .IsRequired()
            .HasDefaultValue(0m);

        builder.Property(e => e.UpdatedAtUtc)
            .IsRequired()
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasIndex(e => new { e.UserId, e.CategoryId })
            .IsUnique()
            .HasDatabaseName("UX_Budgets_UserId_CategoryId");
    }
}
