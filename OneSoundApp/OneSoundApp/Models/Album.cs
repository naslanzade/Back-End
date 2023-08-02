namespace OneSoundApp.Models
{
    public class Album :BaseEntity
    {
        public string AlbumName { get; set; }
        public decimal Price { get; set; }
        public int SingerId { get; set; }
        public Singer Singer { get; set; }
        public ICollection<AlbumImage> Images { get; set; }
        public ICollection<Song> Song { get; set; }
        public string Description { get; set; }
        public int SongCount { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<CartAlbum> CartAlbums { get; set; }
        public ICollection<WishlistAlbum> WishlistAlbums { get; set; }
    }
}
