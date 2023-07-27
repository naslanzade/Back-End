using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.ViewModels.Albums;
using OneSoundApp.ViewModels.Playlist;

namespace OneSoundApp.ViewModels.Shop
{
    public class ShopVM
    {
        public List<Advert> Adverts { get; set; }
        public List<Category> Categories { get; set; }
        public Paginate<AlbumVM> PaginatedDatas { get; set; }
        public Paginate<PlaylistVM> PaginatedData { get; set; }


    }
}
