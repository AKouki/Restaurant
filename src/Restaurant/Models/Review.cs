using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? ReviewerName { get; set; }
        public string? TextEn { get; set; }
        public string? ReviewerNameEn { get; set; }
        [Range(1,5)]
        public int? Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }
    }
}
