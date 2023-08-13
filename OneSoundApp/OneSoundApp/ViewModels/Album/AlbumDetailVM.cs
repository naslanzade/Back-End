using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Albums
{
    public class AlbumDetailVM
    {
        public int Id { get; set; }
        public List<AlbumImage> Images { get; set; }
        public List<Song> Songs { get; set; }
        public string AlbumName { get; set; }
        public string SingerName { get; set; }
        public decimal Price { get; set; }
        
    }
}
