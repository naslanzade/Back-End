using OneSoundApp.Helpers;
using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Playlist
{
    public class PlaylistVM
    {
        public List<Models.Playlist> Playlists { get; set; }
        public List<Advert> Adverts { get; set; }
        public Paginate<Models.Playlist> PaginatedDatas { get; set; }

    }
}
