namespace OneSoundApp.Models
{
    public class Song : BaseEntity
    {
        public string SongName { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int SingerId { get; set; }
        public Singer Singer { get; set; }
        public int? AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
