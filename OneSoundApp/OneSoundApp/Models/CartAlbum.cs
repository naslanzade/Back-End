namespace OneSoundApp.Models
{
    public class CartAlbum :BaseEntity
    {
        public int Count { get; set; }
        public int AlbumId { get; set; }
        public int CartId { get; set; }
        public Album Album { get; set; }
        public Cart Cart { get; set; }

    }
}
