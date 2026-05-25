using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Data.Entities;

namespace WebApp.Data.Persistence.Configurations;

public class IncomeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.ToTable("Incomes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Source)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Notes)
            .HasMaxLength(300);

        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.CreatedAtUtc)
            .IsRequired()
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(e => e.InActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(e => e.CategoryId)
            .HasDatabaseName("IX_Incomes_CategoryId");
    }
}
