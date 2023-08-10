using OneSoundApp.Models;

namespace OneSoundApp.Areas.Admin.ViewModels.Podcast
{
    public class PodcastDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<PodcastImage> Images { get; set; }
        public string AuthorName { get; set; }
        public ICollection<Record> Records { get; set; }
    }
}
