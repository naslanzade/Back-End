using OneSoundApp.Models;

namespace OneSoundApp.Areas.Admin.ViewModels.Album
{
    public class AlbumDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<AlbumImage> Images { get; set; }
        public ICollection<Song> Song { get; set; }
        public decimal Price { get; set; }
        public string SingerName { get; set; }
    }
}
