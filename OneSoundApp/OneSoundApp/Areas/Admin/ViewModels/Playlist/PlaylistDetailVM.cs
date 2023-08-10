using OneSoundApp.Models;

namespace OneSoundApp.Areas.Admin.ViewModels.Playlist
{
    public class PlaylistDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<PlaylistImage> Images { get; set; }
        public ICollection<PlaylistSong> Songs { get; set; }    
    }
}
