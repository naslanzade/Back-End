using System.ComponentModel.DataAnnotations;

namespace OneSoundApp.Areas.Admin.ViewModels.Advert
{
    public class AdvertCreateVM
    {
        [Required]
        public List<IFormFile> Images { get; set; }
       
    }
}
