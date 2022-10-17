using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Areas.Admin.ViewModels
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(200)]
        [DisplayName("Name (English)")]
        public string? NameEn { get; set; }
        [DisplayName("Published")]
        public bool IsPublished { get; set; }
        public int OrderLevel { get; set; }
        public int MenuItemsCount { get; set; }
        public List<MenuItemViewModel>? MenuItems { get; set; }
    }
}
