using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Models;

namespace Restaurant.Data.Configuration
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.Property(m => m.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(m => m.Description)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(m => m.NameEn)
                .HasMaxLength(200);

            builder.Property(m => m.DescriptionEn)
                .HasMaxLength(250);

            builder.Property(m => m.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(m => m.OrderLevel).HasDefaultValue(1000);
        }
    }
}
