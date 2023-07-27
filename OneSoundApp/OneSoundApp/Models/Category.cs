namespace OneSoundApp.Models
{
    public class Category :BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Blog> Blog { get; set; }
        public ICollection<Album> Albums { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
    }
}
