using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Albums
{
    public class AlbumVM
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string AlbumName { get; set; }
        public string SingerName { get; set; }
        public ICollection<Song> Songs { get; set; }    
        public ICollection<AlbumImage> Images { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int SongCount { get; set; }

    }
}
