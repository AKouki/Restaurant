#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Areas.Admin.ViewModels;
using Restaurant.Data;

namespace Restaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel()
            {
                NewReservations = await _db.Reservations.CountAsync(r => r.IsRead == false),
                TodayReservations = await _db.Reservations.CountAsync(r => r.ReservationDateTime.Date == DateTime.Today),
                TotalReservations = await _db.Reservations.CountAsync(),
                TotalReviews = await _db.Reviews.CountAsync(),
                UpcomingReservations = await _db.Reservations
                .Where(r => r.ReservationDateTime.Date >= DateTime.Today)
                .OrderBy(r => r.ReservationDateTime)
                .Take(10)
                .ToListAsync(),
                PendingReservations = await _db.Reservations
                .Where(r => r.IsConfirmed == false)
                .OrderBy(r => r.ReservationDateTime)
                .Take(10)
                .ToListAsync()
            };

            return View(model);
        }
    }
}
