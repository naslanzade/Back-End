namespace OneSoundApp.Models
{
    public class Singer :BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public ICollection<Album> Album { get; set; }
        public ICollection<Song> Song { get; set; }
    }
}
