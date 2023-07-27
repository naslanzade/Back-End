namespace OneSoundApp.Models
{
    public class Playlist :BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int SongCount { get; set; }
        public ICollection<PlaylistSong> Songs { get; set; }
        public ICollection<PlaylistImage> Images { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
