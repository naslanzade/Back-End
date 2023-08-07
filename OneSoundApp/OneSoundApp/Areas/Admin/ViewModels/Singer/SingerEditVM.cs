using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Singer
{
    public class SingerEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }       
        [Required]
        public string Image { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
