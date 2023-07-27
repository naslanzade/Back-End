using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Playlist
{
    public class PlaylistVM
    {
        public int Id { get; set; }
        public string PlaylistName { get; set; }
        public string Name { get; set; }
         public ICollection<PlaylistSong> Songs { get; set; } 
        public ICollection<PlaylistImage> Images { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int SongCount { get; set; }

    }
}
