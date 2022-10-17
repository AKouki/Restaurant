using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Models;

namespace Restaurant.Data.Configuration
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(r => r.FirstName)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(r => r.LastName)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(r => r.Phone).IsUnique();
            builder.Property(r => r.Phone).IsRequired();

            builder.Property(r => r.ReservationDateTime).IsRequired();

            builder.Property(r => r.Guests).IsRequired();

            builder.Property(r => r.Notes).HasMaxLength(1000);
        }
    }
}
