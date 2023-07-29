using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Podcast
{
    public class PodcastDetailVM
    {
        public int Id { get; set; }
        public List<PodcastImage> Images { get; set; }
        public List<Record> Records { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string AuthorName { get; set; }
    }
}
