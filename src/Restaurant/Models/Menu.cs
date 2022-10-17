namespace Restaurant.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameEn { get; set; }
        public bool IsPublished { get; set; } = true;
        public int OrderLevel { get; set; }

        public ICollection<MenuItem>? MenuItems { get; set; }
    }
}
