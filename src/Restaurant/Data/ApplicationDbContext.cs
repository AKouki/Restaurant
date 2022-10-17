using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System.Reflection;

namespace Restaurant.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Menu> Menus => Set<Menu>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Recommendation> Recommendations => Set<Recommendation>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}