using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Models;

namespace Restaurant.Data.Configuration
{
    public class RecommendationConfiguration : IEntityTypeConfiguration<Recommendation>
    {
        public void Configure(EntityTypeBuilder<Recommendation> builder)
        {
            builder.Property(r => r.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(r => r.NameEn)
                .HasMaxLength(200);

            builder.Property(r => r.DescriptionEn)
                .HasMaxLength(250);

            builder.Property(r => r.Price)
                .HasPrecision(18,2)
                .IsRequired();

            builder.Property(r => r.OrderLevel).HasDefaultValue(1000);
        }
    }
}
