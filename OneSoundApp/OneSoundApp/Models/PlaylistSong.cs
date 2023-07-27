namespace OneSoundApp.Models
{
    public class PlaylistSong :BaseEntity
    {
        public string Name { get; set; }
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
