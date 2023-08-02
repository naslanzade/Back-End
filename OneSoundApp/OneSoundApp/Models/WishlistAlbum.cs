namespace OneSoundApp.Models
{
    public class WishlistAlbum:BaseEntity
    {
        public int AlbumId { get; set; }
        public int WishlistId { get; set; }
        public Album Album { get; set; }
        public Wishlist Wishlist { get; set; }
    }
}
