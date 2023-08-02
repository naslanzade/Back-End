namespace OneSoundApp.Models
{
    public class Wishlist :BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<WishlistAlbum> WishlistAlbums { get; set; }
    }
}
