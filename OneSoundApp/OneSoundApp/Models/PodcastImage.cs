namespace OneSoundApp.Models
{
    public class PodcastImage :BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }
    }
}
