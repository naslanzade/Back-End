using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Home
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        public IEnumerable<Album> TopAlbums { get; set; }
        public IEnumerable<Song> Songs { get; set; }
        public IEnumerable<Song> TopSongs { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Singer> Singers { get; set; }
        public string Email { get; set; }


    }
}
