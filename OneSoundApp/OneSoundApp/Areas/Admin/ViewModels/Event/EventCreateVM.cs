using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Event
{
    public class EventCreateVM
    {
        [Required]
        public string EventName { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public List<IFormFile> Images { get; set; }
       
    }
}
