using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Areas.Admin.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(1000)]
        public string? Text { get; set; }
        [Required]
        [Display(Name = "Reviewer Name")]
        public string? ReviewerName { get; set; }

        [StringLength(1000)]
        [Display(Name = "Text (English)")]
        public string? TextEn { get; set; }
        [Display(Name = "Reviewer Name (English)")]
        public string? ReviewerNameEn { get; set; }

        [Required]
        [Range(1,5)]
        public int? Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        [DisplayName("Published")]
        public bool IsPublished { get; set; }
    }
}
