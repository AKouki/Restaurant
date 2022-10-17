using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;

namespace Restaurant.Components
{
    public class RecommendationsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private const int MaxItems = 4;

        public RecommendationsViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _db.Recommendations
                .OrderBy(r => r.OrderLevel)
                .Take(MaxItems)
                .ToListAsync();

            return View(items);
        }
    }
}
