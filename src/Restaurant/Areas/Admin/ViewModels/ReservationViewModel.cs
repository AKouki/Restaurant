using System.ComponentModel.DataAnnotations;

namespace Restaurant.Areas.Admin.ViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string? Phone { get; set; }
        [Required]
        [Display(Name = "Reservation DateTime")]
        public DateTime ReservationDateTime { get; set; }
        [Required]
        [Range(1, 50)]
        public int Guests { get; set; }
        [StringLength(1000)]
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? NotificationSentAt { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsRead { get; set; }
    }
}
