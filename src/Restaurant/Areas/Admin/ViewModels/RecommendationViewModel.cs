using System.ComponentModel.DataAnnotations;

namespace Restaurant.Areas.Admin.ViewModels
{
    public class RecommendationViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
        [Required]
        [StringLength(250)]
        public string? Description { get; set; }

        [StringLength(200)]
        public string? NameEn { get; set; }
        [StringLength(250)]
        public string? DescriptionEn { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal Price { get; set; }
        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }
        public int OrderLevel { get; set; }
    }
}
