namespace OneSoundApp.Models
{
    public class Cart:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<CartAlbum> CartAlbums { get; set; }
    }
}
