using OneSoundApp.Models;

namespace OneSoundApp.Areas.Admin.ViewModels.Podcast
{
    public class PodcastVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PodcastImage> Images { get; set; }
    }
}
