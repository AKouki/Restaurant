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

            var names = new string[] { "Γιώργος Γεωγίου", "Κώστας Αντωνόπουλος", "Βασίλης Παπαδόπουλος", "Μαρία Βασιλείου" };
            var rnd = new Random();

            for (int i = 0; i < names.Length; i++)
            {
                var nameParts = names[i].Split(" ");

                db.Reservations.Add(new Reservation()
                {
                    FirstName = nameParts[0],
                    LastName = nameParts[1],
                    Email = $"useremail{i + 1}@admin.com",
                    Phone = $"694912345{i + 1}",
                    ReservationDateTime = DateTime.Now.AddHours(rnd.Next(1, 7 * 24)),
                    Guests = rnd.Next(2, 5),
                    CreatedAt = DateTime.Now,
                    IsConfirmed = rnd.Next(0, 100) > 50,
                    IsRead = rnd.Next(0, 100) > 20,
                });
            }

            await db.SaveChangesAsync();
        }

        private static async Task CreateMenus(ApplicationDbContext db)
        {
            db.Menus.RemoveRange(db.Menus.ToList());
            await db.SaveChangesAsync();

            db.Menus.Add(new Menu() { Name = "Κυρίως Μενού", NameEn = "Main Dishes", IsPublished = true, OrderLevel = 1001 });
            db.Menus.Add(new Menu() { Name = "Σαλάτες", NameEn = "Salads", IsPublished = true, OrderLevel = 1002 });
            db.Menus.Add(new Menu() { Name = "Κρασιά", NameEn = "Wines", IsPublished = true, OrderLevel = 1003 });
            db.Menus.Add(new Menu() { Name = "Αναψυκτικά", NameEn = "Drinks", IsPublished = true, OrderLevel = 1004 });
            db.Menus.Add(new Menu() { Name = "Επιδόρπια", NameEn = "Desserts", IsPublished = true, OrderLevel = 1005 });

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

            db.Recommendations.Add(new Recommendation()
            {
                Name = "Ριζότο με κοτόπουλο",
                NameEn = "Chicken risotto",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas non.",
                Price = 7.00m,
                OrderLevel = 1001,
                PictureUrl = "demo1.jpg",
            });

            db.Recommendations.Add(new Recommendation()
            {
                Name = "Καρμπονάρα",
                NameEn = "Carbonara",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas non.",
                Price = 8.00m,
                OrderLevel = 1002,
                PictureUrl = "demo2.jpg",
            });

            db.Recommendations.Add(new Recommendation()
            {
                Name = "Φιλέτο κοτόπουλο με λαχανικά",
                NameEn = "Chicken fillet with vegetables",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas non.",
                Price = 8.50m,
                OrderLevel = 1003,
                PictureUrl = "demo3.jpg",
            });

            db.Recommendations.Add(new Recommendation()
            {
                Name = "Κέικ σοκολάτας με φρούτα",
                NameEn = "Chocolate cake with fruits",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas non.",
                Price = 10.00m,
                OrderLevel = 1004,
                PictureUrl = "demo4.jpg",
            });

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
