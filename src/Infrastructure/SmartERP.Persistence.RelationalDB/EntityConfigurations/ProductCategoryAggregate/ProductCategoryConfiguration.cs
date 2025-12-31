using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartERP.Domain.Aggregates.ProductCategoryAggregate;

namespace SmartERP.Persistence.RelationalDB.EntityConfigurations.ProductCategoryAggregate;

internal sealed class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>

{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CategoryName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);
    }
}