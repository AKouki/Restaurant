namespace Restaurant.Models
{
    public class Recommendation
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
        public decimal Price { get; set; }
        public string? PictureUrl { get; set; }
        public int OrderLevel { get; set; }

    }
}
