namespace Restaurant.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public int Guests { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? NotificationSentAt { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsRead { get; set; }
    }
}
