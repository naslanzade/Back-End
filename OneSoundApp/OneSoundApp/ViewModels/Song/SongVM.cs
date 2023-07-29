using OneSoundApp.Helpers;
using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Songs
{
    public class SongVM
    {
        public List<Models.Song> Songs { get; set; }
        public List<Advert> Adverts { get; set; }
        public Paginate<Models.Song> PaginatedDatas { get; set; }
    }
}
