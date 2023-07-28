namespace OneSoundApp.Models
{
    public class Podcast :BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public int? AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<Record> Records { get; set; }
        public ICollection<PodcastImage> Images { get; set; }

    }
}
