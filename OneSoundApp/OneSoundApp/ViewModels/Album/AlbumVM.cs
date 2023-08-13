using OneSoundApp.Helpers;
using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Albums
{
    public class AlbumVM
    {
        public List<Album> Albums { get; set; }       
        public List<Advert> Adverts { get; set; }
        public Paginate<Album> PaginatedDatas { get; set; }

    }
}
