using OneSoundApp.Helpers;
using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Podcast
{
    public class PodcastVM
    {
        public List<Models.Podcast> Podcasts { get; set; }
        public List<Advert> Adverts { get; set; }
        public Paginate<Models.Podcast> PaginatedDatas { get; set; }
    }
}
