using OneSoundApp.Models;

namespace OneSoundApp.Areas.Admin.ViewModels.Album
{
    public class AlbumVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<AlbumImage> Images { get; set; }
    }
}
