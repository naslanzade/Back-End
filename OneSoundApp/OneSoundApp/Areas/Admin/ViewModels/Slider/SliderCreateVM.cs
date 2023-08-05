using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [Required]
        public List<IFormFile> Images { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Title { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Header { get; set; }
    }
}
