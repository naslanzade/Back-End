using OneSoundApp.Models;

namespace OneSoundApp.ViewModels.Wishlist
{
    public class WishlistAlbumDetailVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<Album> Albums { get; set; }
    }
}
