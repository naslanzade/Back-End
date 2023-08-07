using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Singer
{
    public class SingerCreateVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }
    }
}
