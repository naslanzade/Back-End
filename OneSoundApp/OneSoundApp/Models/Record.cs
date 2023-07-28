namespace OneSoundApp.Models
{
    public class Record :BaseEntity
    {
        public string Name { get; set; }
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }
    }
}
