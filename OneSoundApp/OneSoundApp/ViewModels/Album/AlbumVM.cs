using OneSoundApp.Helpers;
using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Albums
{
    public class AlbumVM
    {
        public List<Models.Album> Albums { get; set; }       
        public List<Advert> Adverts { get; set; }
        public Paginate<Models.Album> PaginatedDatas { get; set; }

    }
}
