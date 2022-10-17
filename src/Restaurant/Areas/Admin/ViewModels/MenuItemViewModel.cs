using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Areas.Admin.ViewModels
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
        [Required]
        [StringLength(250)]
        public string? Description { get; set; }

        [StringLength(200)]
        [DisplayName("Name (English)")]
        public string? NameEn { get; set; }

        [StringLength(250)]
        [DisplayName("Description (English)")]
        public string? DescriptionEn { get; set; }

        [Required]
        [Range(0, 1000)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public int OrderLevel { get; set; }
        public int MenuId { get; set; }
        public string? MenuName { get; set; }
    }
}
