using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Playlist
{
    public class PlaylistDetailVM
    {
        public int Id { get; set; }
        public List<PlaylistImage> Images { get; set; }
        public List<PlaylistSong> Songs { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        
    }
}
