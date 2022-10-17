using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;

namespace Restaurant.Pages
{
    public class MenuModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public MenuModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Menu> Menus { get; set; } = new();

        public async Task OnGetAsync()
        {
            Menus = await _db.Menus
                .Where(m => m.IsPublished)
                .OrderBy(m => m.OrderLevel)
                .Include(m => m.MenuItems)
                .ToListAsync();
        }
    }
}
