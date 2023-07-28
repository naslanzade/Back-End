namespace OneSoundApp.Models
{
    public class Category :BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public ICollection<Blog> Blog { get; set; }
        public ICollection<Album> Albums { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<Podcast> Podcasts { get; set; }
    }
}
