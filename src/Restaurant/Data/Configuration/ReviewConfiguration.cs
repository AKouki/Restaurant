using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Models;

namespace Restaurant.Data.Configuration
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(r => r.Text)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(r => r.TextEn)
                .HasMaxLength(1000);

            builder.Property(r => r.Rating).IsRequired();
        }
    }
}
