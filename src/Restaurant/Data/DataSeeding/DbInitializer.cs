using Microsoft.AspNetCore.Identity;
using Restaurant.Models;

namespace Restaurant.Data.DataSeeding
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            await db.Database.EnsureCreatedAsync();

            await CreateUsers(db, userManager);
            await CreateReservations(db);
            await CreateMenus(db);
            await CreateMenuItems(db);
            await CreateRecommendations(db);
            await CreateReviews(db);
        }

        private static async Task CreateUsers(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            var newUser = new ApplicationUser()
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                FullName = "Albion Kouki"
            };

            string password = "admin";

            if (!db.Users.Any(u => u.UserName == newUser.UserName))
                await userManager.CreateAsync(newUser, password);
        }


        private static async Task CreateReservations(ApplicationDbContext db)
        {
            db.Reservations.RemoveRange(db.Reservations.ToList());
            await db.SaveChangesAsync();

            var rnd = new Random();

            db.Reservations.Add(new Reservation()
            {
                FirstName = "Γιώργος",
                LastName = "Γεωργίου",
                Email = "useremail@admin.com",
                Phone = "6949123456",
                ReservationDateTime = DateTime.Now.AddHours(rnd.Next(1, 7 * 24)),
                Guests = 2,
                IsConfirmed = true,
                CreatedAt = DateTime.Now,
                IsRead = true,
            });

            await db.SaveChangesAsync();
        }

        private static async Task CreateMenus(ApplicationDbContext db)
        {
            db.Menus.RemoveRange(db.Menus.ToList());
            await db.SaveChangesAsync();

            db.Menus.Add(new Menu() { Name = "Κυρίως Μενού", NameEn = "Main Dishes", IsPublished = true, OrderLevel = 1000 });
            db.Menus.Add(new Menu() { Name = "Σαλάτες", NameEn = "Salads", IsPublished = true, OrderLevel = 1000 });
            db.Menus.Add(new Menu() { Name = "Κρασιά", NameEn = "Wines", IsPublished = true, OrderLevel = 1000 });
            db.Menus.Add(new Menu() { Name = "Αναψυκτικά", NameEn = "Drinks", IsPublished = true, OrderLevel = 1000 });
            db.Menus.Add(new Menu() { Name = "Επιδόρπια", NameEn = "Desserts", IsPublished = true, OrderLevel = 1000 });

            await db.SaveChangesAsync();
        }

        private static async Task CreateMenuItems(ApplicationDbContext db)
        {
            db.MenuItems.RemoveRange(db.MenuItems.ToList());
            await db.SaveChangesAsync();

            var menus = db.Menus.ToList();

            foreach (var menu in menus)
            {
                for (int i = 0; i < 4; i++)
                {
                    db.MenuItems.Add(new MenuItem()
                    {
                        Name = "Menu Item",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas non.",
                        Price = 10.00m,
                        OrderLevel = 1000,
                        MenuId = menu.Id
                    });
                }
            }

            await db.SaveChangesAsync();
        }

        private static async Task CreateRecommendations(ApplicationDbContext db)
        {
            db.Recommendations.RemoveRange(db.Recommendations.ToList());
            await db.SaveChangesAsync();

            for (int i = 0; i < 4; i++)
            {
                db.Recommendations.Add(new Recommendation()
                {
                    Name = "Menu Item",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas non.",
                    Price = 10.00m,
                    OrderLevel = 1000
                });
            }

            await db.SaveChangesAsync();
        }

        private static async Task CreateReviews(ApplicationDbContext db)
        {
            db.Reviews.RemoveRange(db.Reviews.ToList());
            await db.SaveChangesAsync();

            for (int i = 0; i < 4; i++)
            {
                db.Reviews.Add(new Review()
                {
                    Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas commodo neque vel lectus mollis commodo. Donec porttitor sapien enim, vel porta nisi vulputate sed. Nam.",
                    ReviewerName = "Reviewer Name",
                    Rating = 5,
                    CreatedAt = DateTime.Now,
                    IsPublished = true
                });
            }

            await db.SaveChangesAsync();
        }
    }
}
