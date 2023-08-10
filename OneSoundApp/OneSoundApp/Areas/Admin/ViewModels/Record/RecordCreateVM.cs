using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Records
{
    public class RecordCreateVM
    {
        [Required]
        public string Name { get; set; }
        public int PodcastId { get; set; }
    }
}
