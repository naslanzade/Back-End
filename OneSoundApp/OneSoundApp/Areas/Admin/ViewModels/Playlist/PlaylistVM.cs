using OneSoundApp.Models;

namespace OneSoundApp.Areas.Admin.ViewModels.Playlist
{
    public class PlaylistVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PlaylistImage> Images { get; set; }
    }
}
