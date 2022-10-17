using Restaurant.Models;

namespace Restaurant.Data.DataSeeding
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext db)
        {
            await db.Database.EnsureCreatedAsync();

            await CreateMenus(db);
            await CreateMenuItems(db);
            await CreateRecommendations(db);
            await CreateReviews(db);
        }

        private static async Task CreateMenus(ApplicationDbContext db)
        {
            if (!db.Menus.Any())
            {
                db.Menus.Add(new Menu() { Name = "Κυρίως Μενού", NameEn = "Main Dishes", IsPublished = true, OrderLevel = 1000 });
                db.Menus.Add(new Menu() { Name = "Σαλάτες", NameEn = "Salads", IsPublished = true, OrderLevel = 1000 });
                db.Menus.Add(new Menu() { Name = "Κρασιά", NameEn = "Wines", IsPublished = true, OrderLevel = 1000 });
                db.Menus.Add(new Menu() { Name = "Αναψυκτικά", NameEn = "Drinks", IsPublished = true, OrderLevel = 1000 });
                db.Menus.Add(new Menu() { Name = "Επιδόρπια", NameEn = "Desserts", IsPublished = true, OrderLevel = 1000 });

                await db.SaveChangesAsync();
            }
        }

        private static async Task CreateMenuItems(ApplicationDbContext db)
        {
            var menus = db.Menus.ToList();

            foreach (var menu in menus)
            {
                var totalItems = db.MenuItems.Count(m => m.MenuId == menu.Id);
                if (totalItems == 0)
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
            }

            await db.SaveChangesAsync();
        }

        private static async Task CreateRecommendations(ApplicationDbContext db)
        {
            if (!db.Recommendations.Any())
            {
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
        }

        private static async Task CreateReviews(ApplicationDbContext db)
        {
            if (!db.Reviews.Any())
            {
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
}
