using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Record
{
    public class RecordEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int PodcastId { get; set; }
    }
}
