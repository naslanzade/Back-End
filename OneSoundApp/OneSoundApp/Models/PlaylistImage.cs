namespace OneSoundApp.Models
{
    public class PlaylistImage :BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
