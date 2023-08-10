using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Event
{
    public class EventEditVM
    {
        public int Id { get; set; }
        [Required]
        public string EventName { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Image { get; set; }
        public IFormFile? NewImage { get; set; }
      
    }
}
