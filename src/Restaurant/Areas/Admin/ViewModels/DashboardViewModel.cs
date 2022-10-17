using Restaurant.Models;

namespace Restaurant.Areas.Admin.ViewModels
{
    public class DashboardViewModel
    {
        public int NewReservations { get; set; }
        public int TodayReservations { get; set; }
        public int TotalReservations { get; set; }
        public int TotalReviews { get; set; }
        public List<Reservation> UpcomingReservations { get; set; } = new();
        public List<Reservation> PendingReservations { get; set; } = new();
    }
}
