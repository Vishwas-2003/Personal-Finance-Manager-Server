using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Data.Entities;

namespace WebApp.Data.Persistence.Configurations;

public class CategoryTypeConfiguration : IEntityTypeConfiguration<CategoryType>
{
    public void Configure(EntityTypeBuilder<CategoryType> builder)
    {
        builder.HasData(SeedData.CategoryTypeData);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.HasMany(e => e.Categories)
            .WithOne(e => e.CategoryType)
            .HasForeignKey(e => e.CategoryTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
