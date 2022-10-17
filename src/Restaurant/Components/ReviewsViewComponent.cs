using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;

namespace Restaurant.Components
{
    public class ReviewsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private const int MaxItems = 10;

        public ReviewsViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _db.Reviews
                .Where(r => r.IsPublished)
                .OrderByDescending(x => x.CreatedAt)
                .Take(MaxItems)
                .ToListAsync();

            return View(items);
        }
    }
}
