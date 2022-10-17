using Restaurant.Data.DataSeeding;
using Restaurant.Data;

namespace Restaurant.Extensions
{
    public static class DbInitializerExtensions
    {
        public static IHost SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    DbInitializer.Initialize(context).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error initializing the database" + ex.Message);
                }
            }

            return host;
        }
    }
}
