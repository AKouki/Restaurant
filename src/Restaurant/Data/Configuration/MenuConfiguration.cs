using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Models;

namespace Restaurant.Data.Configuration
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.Property(m => m.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(m => m.NameEn)
                .HasMaxLength(200);

            builder.Property(m => m.OrderLevel).HasDefaultValue(1000);

            builder.HasMany(m => m.MenuItems)
                .WithOne(i => i.Menu)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
