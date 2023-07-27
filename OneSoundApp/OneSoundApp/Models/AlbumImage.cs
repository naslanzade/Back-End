namespace OneSoundApp.Models
{
    public class AlbumImage :BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
