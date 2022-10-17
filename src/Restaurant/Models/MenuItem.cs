namespace Restaurant.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
        public decimal Price { get; set; }
        public int OrderLevel { get; set; }

        public int MenuId { get; set; }
        public Menu? Menu { get; set; }
    }
}
